using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private const string MenuSceneName = "MenuScene";

	public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
        
        AuthState authState = await AuthenticationWrapper.DoAuth();
        return (authState == AuthState.Authenticated);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
}
