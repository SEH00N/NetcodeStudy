using System;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{
	public ulong clientID;
    public int colorID;

    public bool Equals(PlayerData other)
    {
        bool equal = this.clientID == other.clientID;
        equal &= this.colorID == other.colorID;
        return equal;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientID);
        serializer.SerializeValue(ref colorID);
    }
}
