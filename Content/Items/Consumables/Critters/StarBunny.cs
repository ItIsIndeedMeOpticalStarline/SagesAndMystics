using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Consumables.Critters
{
    public class StarBunny : ModItem, IGlowmask
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Bunny);
            Item.makeNPC = ModContent.NPCType<NPCs.Critters.StarBunny>();

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 10);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(Item.Center, new Vector3(0.816f, 0.78f, 0.945f));
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 10;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => 255;
    }
}
