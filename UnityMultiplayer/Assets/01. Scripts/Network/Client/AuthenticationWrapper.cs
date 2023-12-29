using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;

    public static async Task<AuthState> DoAuth(int retries = 5)
    {
        if(AuthState == AuthState.Authenticated)
            return AuthState;

        if(AuthState == AuthState.Authenticating)
        {
            await Authenticating();
            return AuthState;
        }

        await SignInAnonymouslyAsync(retries);

        return AuthState;
    }

    private static async Task<AuthState> Authenticating()
    {
        while(AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
            await Task.Delay(200);

        return AuthState;
    }

    private static async Task SignInAnonymouslyAsync(int retries)
    {
        AuthState = AuthState.Authenticating;
        int tries = 0;
        while(AuthState == AuthState.Authenticating && tries < retries)
        {
            try {
                IAuthenticationService authService = AuthenticationService.Instance;
                await authService.SignInAnonymouslyAsync();
                if(authService.IsSignedIn && authService.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
            } catch {
                AuthState = AuthState.Error;
            }
            // catch(AuthenticationException err) {
            //     AuthState = AuthState.Error;
            // } catch(RequestFailedException err) {
            //     AuthState = AuthState.Error;
            // }

            tries++;
            await Task.Delay(1000);
        }

        if(AuthState != AuthState.Authenticated)
            AuthState = AuthState.TimeOut;
    }
}
