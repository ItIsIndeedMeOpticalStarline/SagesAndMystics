using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System.IO;
using SagesAndMystics.Common;

namespace SagesAndMystics
{
    public class NetHelper
    {
        private enum MessageType : byte
        {
            SageHelpTextUpdate
        }

        public static void HandlePacket(BinaryReader reader, int sender)
        {
            MessageType type = (MessageType)reader.ReadByte();

            switch (type)
            {
                case MessageType.SageHelpTextUpdate:
                    {
                        ClientReceiveSageTextUpdate();
                    }
                    break;
            }
        }

        public static void SendSageTextUpdate()
        {
            if (Main.netMode != NetmodeID.Server)
                return;

            ModPacket packet = ModContent.GetInstance<SagesAndMystics>().GetPacket();
            packet.Write((byte)MessageType.SageHelpTextUpdate);
            packet.Send();
        }

        public static void ClientReceiveSageTextUpdate()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            SageTextTracking.SetPendingText();
        }
    }
}
