using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MonoMod.Cil;
using System;
using Mono.Cecil.Cil;

namespace SagesAndMystics.Content.Items.Tools.Hardmode
{
    public class Boline : ModItem
    {
        public override void Load()
        {
            IL_Player.ItemCheck_OwnerOnlyCode += HookCutTile;
        }

        private static void HookCutTile(ILContext il)
        {
            try
            {
                ILCursor c = new ILCursor(il);

                ILLabel labelIL_049CPlusOne = il.DefineLabel();

                // Find where we are checking for the item type to continue running the collision code
                c.GotoNext(i => i.MatchLdcI4(3779));
                c.Index++;
                // Place beq.s before bne.un.s
                c.Emit(OpCodes.Beq_S, labelIL_049CPlusOne);
                // Push item type onto the stack
                c.Emit(OpCodes.Ldarg_2).Emit(OpCodes.Ldfld, typeof(Item).GetField("type"));
                // Push our type onto the stack
                c.Emit(OpCodes.Call, typeof(ModContent).GetMethod("ItemType", new Type[] { }).MakeGenericMethod(typeof(Boline)));
                // No need to add an aditional branch instruction since the bne.un.s is still here

                // Set label to be after IL_049C
                c.GotoNext(i => i.MatchLdarg0());
                c.MarkLabel(labelIL_049CPlusOne);

                ILLabel labelIL_0502PlusOne = il.DefineLabel();

                // Find where we are checking for item types to break tiles
                c.GotoNext(i => i.MatchLdcI4(4821));
                c.Index++;
                // Place beq.s before bne.un.s
                c.Emit(OpCodes.Beq_S, labelIL_0502PlusOne);
                // Push item type onto the stack
                c.Emit(OpCodes.Ldarg_2).Emit(OpCodes.Ldfld, typeof(Item).GetField("type"));
                // Push our type onto the stack
                c.Emit(OpCodes.Call, typeof(ModContent).GetMethod("ItemType", new Type[] { }).MakeGenericMethod(typeof(Boline)));
                // No need to add an aditional branch instruction since the bne.un.s is still here

                // Set label to be after IL_0502
                c.GotoNext(i => i.MatchLdarg0());
                c.MarkLabel(labelIL_0502PlusOne);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<SagesAndMystics>(), il, e);
            }
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 36;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.value = Item.buyPrice(gold: 2, silver: 40);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Pearlwood, 4)
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
