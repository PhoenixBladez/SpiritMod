using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Gores
{
    public class GreenLeaf : ModGore
    {
        public override void OnSpawn(Gore gore) {
            gore.numFrames = 8;
            gore.behindTiles = true;
            gore.timeLeft = Gore.goreTime * 3;
        }

        public override bool Update(Gore gore) {
            gore.velocity.X = Main.windSpeed * 23;
            gore.rotation = gore.velocity.ToRotation() + MathHelper.PiOver2;
            gore.position += gore.velocity;
            if(Collision.SolidCollision(gore.position, 2, 2)) {
                gore.active = false;
                return false;
            }
            gore.frameCounter++;
            if(gore.frameCounter > 7) {
                gore.frameCounter = 0;
                gore.frame++;
                if(gore.frame > 7) gore.frame = 0;
            }
            return false;
        }
    }
}
