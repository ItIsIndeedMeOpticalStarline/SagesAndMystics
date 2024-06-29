using SagesAndMystics.Content.Tiles.Crafting;
using SagesAndMystics.Content.Tiles.Furniture;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SagesAndMystics.Content.Items.Weapons.PreHardmode.Classless
{
    public class Athame : ModItem
    {
        public static readonly int[] DamagedByAthame =
        [
            NPCID.Ghost,
            NPCID.Wraith,
            NPCID.PossessedArmor
        ];

        private int consecrated;

        public bool Consecrated 
        { 
            get => consecrated == 0 ? false : true; 
            set => consecrated = value == false ? 0 : 1; 
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;

            Item.damage = 80;
            Item.DamageType = DamageClass.Default;
            Item.knockBack = 3f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.buyPrice(silver: 18);
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltip;
            if (Consecrated)
                tooltip = new TooltipLine(Mod, "SagesAndMystics: Consecrated", this.GetLocalizedValue("Consecrated"));
            else
                tooltip = new TooltipLine(Mod, "SagesAndMystics: Consecrated", this.GetLocalizedValue("NotConsecrated"));

            int vanillaLastTooltipIndex = tooltips.FindIndex(vanillaLine => vanillaLine.Name.StartsWith("Tooltip1"));
            tooltips.Insert(vanillaLastTooltipIndex + 1, tooltip);
        }

        public override bool? CanHitNPC(Player player, NPC target)
        {
            if (Consecrated && DamagedByAthame.Contains(target.type))
                return true;

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AshWood, 4)
                .AddRecipeGroup(RecipeGroupID.IronBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("consecrated", consecrated);
        }

        public override void LoadData(TagCompound tag)
        {
            consecrated = tag.GetAsInt("consecrated");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(consecrated);
        }

        public override void NetReceive(BinaryReader reader)
        {
            consecrated = reader.ReadInt32();
        }
    }
}
