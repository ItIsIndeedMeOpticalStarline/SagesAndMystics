using SagesAndMystics.Content.NPCs.TownNPCs;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Common
{
    public class SageTextTracking : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            bool newText = false;

            if (npc.type == NPCID.KingSlime)
                newText = true;

            if (newText)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                    SetPendingText();
                else
                    NetHelper.SendSageTextUpdate();
            }
        }

        public static void SetPendingText()
        {
            foreach (Sage golem in Main.npc.Take(Main.maxNPCs).Where(n => n.active && n.ModNPC is Sage).Select(n => n.ModNPC as Sage))
                golem.pendingNewHelpTextCheck = true;
        }
    }
}
