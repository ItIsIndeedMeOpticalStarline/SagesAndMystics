using Microsoft.Xna.Framework;
using SagesAndMystics.Content.Items.Tools.Hardmode;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Common
{
    public class SagesAndMysticsGlobalTile : GlobalTile
    {
        public override void Drop(int i, int j, int type)
        {
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Boline>() && Main.LocalPlayer.itemAnimation > 0)
            {
                Tile tile = Framing.GetTileSafely(i, j);

                int itemType = ItemID.None;
                if (type == TileID.BloomingHerbs) // Only Shiverthorn is harvestable exclusively at the blooming stage
                {
                    switch (tile.TileFrameX)
                    {
                        // Other herbs are here as a big sanity check
                        case 18 * 0: { itemType = ItemID.Daybloom; } break;
                        case 18 * 1: { itemType = ItemID.Moonglow; } break;
                        case 18 * 2: { itemType = ItemID.Blinkroot; } break;
                        case 18 * 3: { itemType = ItemID.Deathweed; } break;
                        case 18 * 4: { itemType = ItemID.Waterleaf; } break;
                        case 18 * 5: { itemType = ItemID.Fireblossom; } break;
                        case 18 * 6: { itemType = ItemID.Shiverthorn; } break;
                    }

                    if (Main.rand.NextBool())
                        Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16f, j * 16f), itemType);
                }
                else if (type == TileID.MatureHerbs) // All other herbs are harvestable at the mature stage
                {
                    switch (tile.TileFrameX)
                    {
                        case 18 * 0: { itemType = ItemID.Daybloom; } break;
                        case 18 * 1: { itemType = ItemID.Moonglow; } break;
                        case 18 * 2: { itemType = ItemID.Blinkroot; } break;
                        case 18 * 3: { itemType = ItemID.Deathweed; } break;
                        case 18 * 4: { itemType = ItemID.Waterleaf; } break;
                        case 18 * 5: { itemType = ItemID.Fireblossom; } break;
                        //case 18 * 6: { itemType = ItemID.Shiverthorn; } break; // Don't give the player Shiverthorn early
                    }

                    if (Main.rand.NextBool())
                        Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16f, j * 16f), itemType);
                }
            }
        }
    }
}
