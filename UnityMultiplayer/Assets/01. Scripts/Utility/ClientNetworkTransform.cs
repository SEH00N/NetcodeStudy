using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    // 해당 오브젝트가 NGO에 의해 스폰되었을 때 호출되는 메소드
    // Awake에서 초기화하면 네트워크 지연 때문에 문제가 생길 수 있음
    // Awake대신에 사용하면 될 듯
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // 트랜스폼 수정 권한 부여
        CanCommitToTransform = IsOwner; // 해당 오브젝트가 해당 클라이언트의 오브젝트라면 직접 수정할 수 있도록 함

        // IsOwner => 해당 오브젝트가 자기 자신의 오브젝트인지를 나타냄
        // CanCommitToTransform => 해당 NetworkTransform 수정 권한
    }

    protected override void Update()
    {
        CanCommitToTransform = IsOwner; // 변수가 내부적으로 수정될 수도 있기 때문에 매번 업데이트 해주기
        base.Update();

        if(NetworkManager == null)
            return;

        if(NetworkManager.IsConnectedClient == false || NetworkManager.IsListening == false)
            return;

        if(CanCommitToTransform == false)
            return;

        // 조작 권한이 해당 클라이언트에게 있기 때문에 변경된 점을 서버에 직접 동기화 시켜야 함
        // dirtyTime은 마지막으로 동기화 된 시간과 비교하기 위한 시간
        // 클라를 먼저 움직이고 그 후에 동기화를 하는 방식
        TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
    }

    // 서버가 해당 클라를 신뢰할 수 있는지에 대한 함수
    // false를 반환하면 클라이언트가 해당 Transform의 조작 권한을 갖게 됨
    protected override bool OnIsServerAuthoritative() => false;
}
