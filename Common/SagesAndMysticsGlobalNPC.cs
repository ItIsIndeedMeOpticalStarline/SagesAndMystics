using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Common
{
    public class SagesAndMysticsGlobalNPC : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (Main.LocalPlayer.GetModPlayer<SagesAndMysticsPlayer>().saltPouch && npc.type == NPCID.Ghost)
                npc.active = false;
        }
    }
}
