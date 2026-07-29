using Random = UnityEngine.Random;

namespace BulletBang
{
    /// <summary>
    /// Stores process-local player preferences that must be available before a
    /// network object exists. This class never represents authoritative game state.
    /// </summary>
    public static class LocalPlayerData
    {
        private static string _nickName;

        /// <summary>
        /// Gets or sets the local player's display name. A readable random name is
        /// generated lazily when no name was entered.
        /// </summary>
        public static string NickName
        {
            set => _nickName = value;
            get
            {
                if (string.IsNullOrWhiteSpace(_nickName))
                {
                    var rngPlayerNumber = Random.Range(0, 9999);
                    _nickName = $"Player {rngPlayerNumber.ToString("0000")}";
                }
                return _nickName;
            }
        }
    }
}
