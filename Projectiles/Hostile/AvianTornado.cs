
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
    public class AvianNado : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Gustnado");
        }

        public override void SetDefaults() {
            projectile.width = 24;
            projectile.height = 100;
            projectile.hostile = true;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }
        public override void AI() {
            float num1178 = 900f;
            if(projectile.type == 657) {
                num1178 = 300f;
            }
            projectile.ai[0] += 1f;
            if(projectile.ai[0] >= num1178) {
                projectile.Kill();
            }
            if(projectile.type == 656 && projectile.localAI[0] >= 30f) {
                projectile.damage = 0;
                if(projectile.ai[0] < num1178 - 120f) {
                    float num1177 = projectile.ai[0] % 60f;
                    projectile.ai[0] = num1178 - 120f + num1177;
                    projectile.netUpdate = true;
                }
            }
            float num1176 = 15f;
            float num1175 = 15f;
            Point point10 = projectile.Center.ToTileCoordinates();
            int num1851 = default(int);
            int num1852 = default(int);
            Collision.ExpandVertically(point10.X, point10.Y, out num1851, out num1852, (int)num1176, (int)num1175);
            int num2475 = num1851;
            num1851 = num2475 + 1;
            num2475 = num1852;
            num1852 = num2475 - 1;
            Vector2 value93 = new Vector2((float)point10.X, (float)num1851) * 16f + new Vector2(8f);
            Vector2 value92 = new Vector2((float)point10.X, (float)num1852) * 16f + new Vector2(8f);
            Vector2 vector152 = Vector2.Lerp(value93, value92, 0.5f);
            Vector2 value159 = new Vector2(0f, value92.Y - value93.Y);
            value159.X = value159.Y * 0.2f;
            projectile.width = (int)(value159.X * 0.65f);
            projectile.height = (int)value159.Y;
            projectile.Center = vector152;

            if(projectile.ai[0] < num1178 - 120f) {
                for(int num1172 = 0; num1172 < 1; num1172 = num2475 + 1) {
                    float value91 = -0.5f;
                    float value90 = 0.9f;
                    float amount5 = Main.rand.NextFloat();
                    Vector2 value160 = new Vector2(MathHelper.Lerp(0.1f, 1f, Main.rand.NextFloat()), MathHelper.Lerp(value91, value90, amount5));
                    value160.X *= MathHelper.Lerp(2.2f, 0.6f, amount5);
                    value160.X *= -1f;
                    Vector2 value89 = new Vector2(6f, 10f);
                    Vector2 position7 = vector152 + value159 * value160 * 0.5f + value89;
                    Dust[] dust122 = Main.dust;
                    Vector2 position221 = position7;
                    Color newColor5 = default(Color);
                    Dust dust44 = dust122[Dust.NewDust(position221, 0, 0, DustID.SilverCoin, 0f, 0f, 0, newColor5, 1f)];
                    dust44.position = position7;
                    dust44.noLight = true;
                    dust44.customData = vector152 + value89;
                    dust44.fadeIn = 1f;
                    dust44.scale = 0.3f;
                    if(value160.X > -1.2f) {
                        dust44.velocity.X = 1f + Main.rand.NextFloat();
                    }
                    dust44.velocity.Y = Main.rand.NextFloat() * -0.5f - 1f;
                    num2475 = num1172;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            float num391 = 900f;
            if(projectile.type == 657) {
                num391 = 300f;
            }
            float num390 = 15f;
            float num389 = 15f;
            float num388 = projectile.ai[0];
            float scale6 = MathHelper.Clamp(num388 / 30f, 0f, 1f);
            if(num388 > num391 - 60f) {
                scale6 = MathHelper.Lerp(1f, 0f, (num388 - (num391 - 60f)) / 60f);
            }
            Microsoft.Xna.Framework.Point point5 = projectile.Center.ToTileCoordinates();
            int num571 = default(int);
            int num572 = default(int);
            Collision.ExpandVertically(point5.X, point5.Y, out num571, out num572, (int)num390, (int)num389);
            num571++;
            num572--;
            float num387 = 0.2f;
            Vector2 value13 = new Vector2((float)point5.X, (float)num571) * 16f + new Vector2(8f);
            Vector2 value12 = new Vector2((float)point5.X, (float)num572) * 16f + new Vector2(8f);
            Vector2.Lerp(value13, value12, 0.5f);
            Vector2 vector46 = new Vector2(0f, value12.Y - value13.Y);
            vector46.X = vector46.Y * num387;
            new Vector2(value13.X - vector46.X / 2f, value13.Y);
            Texture2D texture2D12 = Main.projectileTexture[projectile.type];
            Microsoft.Xna.Framework.Rectangle rectangle5 = texture2D12.Frame(1, 1, 0, 0);
            Vector2 origin6 = rectangle5.Size() / 2f;
            float num386 = -0.06283186f * num388;
            Vector2 spinningpoint2 = Vector2.UnitY.RotatedBy((double)(num388 * 0.1f), default(Vector2));
            float num384 = 0f;
            float num383 = 5.1f;
            Microsoft.Xna.Framework.Color value11 = new Microsoft.Xna.Framework.Color(214, 201, 176);
            for(float num382 = (float)(int)value12.Y; num382 > (float)(int)value13.Y; num382 -= num383) {
                num384 += num383;
                float num381 = num384 / vector46.Y;
                float num380 = num384 * 6.28318548f / -20f;
                float num379 = num381 - 0.15f;
                Vector2 vector47 = spinningpoint2.RotatedBy((double)num380, default(Vector2));
                Vector2 value57 = new Vector2(0f, num381 + 1f);
                value57.X = value57.Y * num387;
                Microsoft.Xna.Framework.Color color70 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Transparent, value11, num381 * 2f);
                if(num381 > 0.5f) {
                    color70 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Transparent, value11, 2f - num381 * 2f);
                }
                color70.A = (byte)((float)(int)color70.A * 0.5f);
                color70 *= scale6;
                vector47 *= value57 * 100f;
                vector47.Y = 0f;
                vector47.X = 0f;
                vector47 += new Vector2(value12.X, num382) - Main.screenPosition;
                spriteBatch.Draw(texture2D12, vector47, rectangle5, color70, num386 + num380, origin6, 1f + num379, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit) {
            target.velocity.Y -= 8f;
        }
    }
}
