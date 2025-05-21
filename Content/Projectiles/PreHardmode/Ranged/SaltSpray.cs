using Terraria;
using Terraria.ModLoader;
using SagesAndMystics.Content.Dusts;
using SagesAndMystics.Content.Items.Weapons.PreHardmode.Classless;
using SagesAndMystics.Content.Tiles.Crafting;
using SagesAndMystics.Content.Tiles.Furniture;
using System;
using Terraria.ID;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Ranged
{
    public class SaltSpray : ModProjectile
    {
        public override string Texture => "SagesAndMystics/Assets/Textures/EmptyTexture";

        public bool Initalized
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value == true ? 1f : 0f;
        }

        public bool IsKosher
        {
            get => Projectile.damage == 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 45;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (!Initalized)
            {
                for (int i = 0; i < 30; i++)
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SaltDust>(), Projectile.velocity.X, Projectile.velocity.Y, 50);

                Initalized = true;
            }

            Projectile.velocity *= 0.95f;

            Projectile.velocity.Y += 0.1f;

            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;

            if (IsKosher)
            {
                foreach (Item item in Main.item)
                {
                    if (!item.active)
                        continue;

                    if (item.type == ModContent.ItemType<Athame>() && item.ModItem is Athame athame && Projectile.Hitbox.Intersects(item.Hitbox) && !athame.Consecrated)
                    {
                        int tileX = (int)(athame.Item.Center.X / 16);
                        int tileY = (int)(athame.Item.Center.Y / 16);

                        bool foundAltar = false;
                        bool foundWitchsCandle = false;

                        int prevTileX = tileX - 2;
                        prevTileX = Math.Clamp(prevTileX, 0, Main.maxTilesX);

                        int nextTileX = tileX + 2;
                        nextTileX = Math.Clamp(nextTileX, 0, Main.maxTilesX);

                        int prevTileY = tileY - 2;
                        prevTileY = Math.Clamp(prevTileY, 0, Main.maxTilesY);

                        int nextTileY = tileY + 2;
                        nextTileY = Math.Clamp(nextTileY, 0, Main.maxTilesY);

                        for (int i = prevTileX; i <= nextTileX; i++)
                        {
                            for (int j = prevTileY; j <= nextTileY; j++)
                            {
                                Tile tile = Framing.GetTileSafely(i, j);

                                if (tile.HasTile)
                                {
                                    if (tile.TileType == ModContent.TileType<Altar>())
                                        foundAltar = true;
                                    else if (tile.TileType == ModContent.TileType<WitchsCandle>())
                                        foundWitchsCandle = true;
                                }
                            }
                        }

                        if (foundAltar && foundWitchsCandle)
                        {
                            athame.Consecrated = true;

                            for (int j = 0; j < 15; j++)
                            {
                                Dust.NewDust(athame.Item.position, athame.Item.width, athame.Item.height, DustID.Enchanted_Gold);
                            }
                        }
                    }
                }
            }
        }
    }
}
