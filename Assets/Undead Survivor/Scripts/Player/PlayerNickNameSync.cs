using Fusion;
using UnityEngine;

namespace Undead_Survivor.Scripts.Player
{
    public class PlayerNickNameSync : NetworkBehaviour
    {
        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                RPC_ChangeNickName(PlayerPrefs.GetString("Name"), Runner.LocalPlayer);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_ChangeNickName(string nickName, PlayerRef playerRef)
        {
            PlayerDataSystem._singleton.SetName(playerRef, nickName);
        }
    }
}