using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using SagesAndMystics.Common.Glowmask;

namespace SagesAndMystics.Content.Items.Materials
{
    public class StarBunnyFur : ModItem, IGlowmask
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;

            Item.maxStack = Item.CommonMaxStack;

            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Blue;
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX { get { return 10; } }

        public int GlowmaskOffsetY { get { return 0; } }

        public int GlowmaskAlpha { get { return 255; } }
    }
}
