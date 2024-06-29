using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;

namespace SagesAndMystics
{
	public class SagesAndMystics : Mod
	{
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Asset<Effect> screenRef = Assets.Request<Effect>("Content/Effects/ShockwaveEffect");
                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave"].Load();
            }
        }
    }
}
