using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Messages;

namespace JEM.QNet.UnityEngine.Extensions
{
    internal static class QNetPeerExtension
    {
        /// <summary>
        ///     Generates outgoing message.
        /// </summary>
        public static QNetMessageWriter GenerateOutgoingMessage(this QNetPeer peer, QNetUnityLocalHeader messageHeader,
            params object[] obj)
        {
            return peer.GenerateOutgoingMessage((ushort) messageHeader, obj);
        }
    }
}