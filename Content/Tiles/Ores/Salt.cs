using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SagesAndMystics.Content.Dusts;

namespace SagesAndMystics.Content.Tiles.Ores
{
    public class Salt : ModTile
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(218, 218, 218), this.GetLocalization("MapEntry"));
            RegisterItemDrop(ModContent.ItemType<Items.Materials.Ores.Salt>());

            TileID.Sets.Ore[Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileStone[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileOreFinderPriority[Type] = 150;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 900;
            Main.tileMerge[Type][TileID.Mud] = true;
            Main.tileMerge[TileID.Mud][Type] = true;

            DustType = ModContent.DustType<SaltDust>();
            HitSound = SoundID.Tink;
        }
    }
}
