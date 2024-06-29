using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Content.Items.Materials.Herbs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SagesAndMystics.Content.Tiles.Herbs
{
    public class MandrakeRoot : ModTile
    {
        private const int FrameWidth = 18;

        public enum PlantStage : byte
        {
            Planted,
            Growing,
            Grown
        }

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.IgnoredInHouseScore[Type] = true;
            TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(98, 41, 14), name);

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newTile.AnchorValidTiles = new int[] {
                TileID.Sandstone
            };
            TileObjectData.newTile.AnchorAlternateTiles = new int[] {
                TileID.ClayPot,
                TileID.PlanterBox
            };
            TileObjectData.addTile(Type);

            HitSound = SoundID.Grass;
            DustType = DustID.Ambient_DarkBrown;
        }

        public override bool CanPlace(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            if (tile.HasTile)
            {
                if (tile.TileType == Type)
                {
                    PlantStage stage = GetStage(i, j);

                    return stage == PlantStage.Grown;
                }
                else
                {
                    if (Main.tileCut[tile.TileType] || 
                        TileID.Sets.BreakableWhenPlacing[tile.TileType] || 
                        tile.TileType == TileID.WaterDrip || 
                        tile.TileType == TileID.LavaDrip || 
                        tile.TileType == TileID.HoneyDrip || 
                        tile.TileType == TileID.SandDrip)
                    {
                        bool foliageGrass = tile.TileType == TileID.Plants || tile.TileType == TileID.Plants2;
                        bool moddedFoliage = tile.TileType >= TileID.Count && (Main.tileCut[tile.TileType] || TileID.Sets.BreakableWhenPlacing[tile.TileType]);
                        bool harvestableVanillaHerb = Main.tileAlch[tile.TileType] && WorldGen.IsHarvestableHerbWithSeed(tile.TileType, tile.TileFrameX / FrameWidth);

                        if (foliageGrass || moddedFoliage || harvestableVanillaHerb)
                        {
                            WorldGen.KillTile(i, j);
                            if (!tile.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);

                            return true;
                        }
                    }

                    return false;
                }
            }

            return true;
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 0)
                spriteEffects = SpriteEffects.FlipHorizontally;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = -2;
        }

        public override bool CanDrop(int i, int j)
        {
            PlantStage stage = GetStage(i, j);

            if (stage == PlantStage.Planted)
                return false;

            return true;
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            PlantStage stage = GetStage(i, j);

            Vector2 worldPosition = new Vector2(i, j).ToWorldCoordinates();

            if (stage == PlantStage.Grown)
                NPC.NewNPC(new EntitySource_TileBreak(i, j), (int)worldPosition.X, (int)worldPosition.Y, ModContent.NPCType<NPCs.PreHardmode.MandrakeRoot>());

            yield return new Item(ItemID.None, 0);
        }

        public override bool IsTileSpelunkable(int i, int j)
        {
            PlantStage stage = GetStage(i, j);

            return stage == PlantStage.Grown;
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            PlantStage stage = GetStage(i, j);

            if (stage != PlantStage.Grown)
            {
                tile.TileFrameX += FrameWidth;

                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendTileSquare(-1, i, j, 1);
            }
        }

        private static PlantStage GetStage(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            return (PlantStage)(tile.TileFrameX / FrameWidth);
        }
    }
}
