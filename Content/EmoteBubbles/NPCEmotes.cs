using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.EmoteBubbles
{
    public abstract class ModTownEmote : ModEmoteBubble
    {
        public override string Texture => "SagesAndMystics/Content/EmoteBubbles/NPCEmotes";

        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.Town);
        }

        public virtual int Row => 0;

        public override Rectangle? GetFrame()
        {
            return new Rectangle(EmoteBubble.frame * 34, 28 * Row, 34, 28);
        }

        public override Rectangle? GetFrameInEmoteMenu(int frame, int frameCounter)
        {
            return new Rectangle(frame * 34, 28 * Row, 34, 28);
        }
    }

    public class SageEmote : ModTownEmote
    {
        public override int Row => 0;
    }
}
