using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.ChargeMeter;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.AccursedBlade
{
    public class AccursedBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Accursed Blade");
            Tooltip.SetDefault("Kill enemies and collect their souls to build up charge \n Right click to release charge as a cursed bolt");
            SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.rare = ItemRarityID.Green;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3f; 
            item.useTime = 25;
            item.useAnimation = 25;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.damage = 15;
            item.width = 30;
            item.height = 30;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<AccursedBolt>();
            item.shootSpeed = 9;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            GlowmaskUtils.DrawItemGlowMaskWorld(Main.spriteBatch, item, mod.GetTexture(Texture.Remove(0, "Starjinx/".Length) + "_glow"), rotation, scale);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if(player.GetModPlayer<AccursedBladePlayer>().charge == 0)
                    return false;

                item.useStyle = ItemUseStyleID.HoldingOut;
                Item.staff[item.type] = true;
                item.noMelee = true;
                return true;
            }
            item.useStyle = ItemUseStyleID.SwingThrow;
            Item.staff[item.type] = false;
            item.noMelee = false;
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            if (player.altFunctionUse != 2 || player.GetModPlayer<AccursedBladePlayer>().charge == 0)
            {
                return false;
            }
            damage = 10 + (int)(player.GetModPlayer<AccursedBladePlayer>().charge * 25);
            Projectile.NewProjectile(position, new Vector2(speedX,speedY), type, damage, knockBack, player.whoAmI, player.GetModPlayer<AccursedBladePlayer>().charge);
            player.GetModPlayer<AccursedBladePlayer>().charge = 0;
            Main.PlaySound(SoundID.NPCKilled, (int)player.position.X, (int)player.position.Y, 52, 1.2f, -0.3f);
            return true;
        }
        public override void HoldItem(Player player)
        {
			ChargeMeterPlayer modPlayer = player.GetModPlayer<ChargeMeterPlayer>();
            modPlayer.chargeMeter.drawMeter = true;
            modPlayer.chargeMeter.charge = player.GetModPlayer<AccursedBladePlayer>().charge;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (target.life <= 0)
            {
               Item.NewItem((int)target.position.X, (int)target.position.Y - 20, target.width, target.height, mod.ItemType("AccursedSoul"));
            }
        }
    }
    public class AccursedSoul: ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Accursed Soul");
			Tooltip.SetDefault("You shouldn't see this");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 7));
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 20;
			item.maxStack = 1;
			item.alpha = 0;
			ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
		}

		public override bool ItemSpace(Player player)
		{
			return true;
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange = 0;
		}

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            item.alpha ++;
            if (item.alpha > 255)
                item.active = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.White, Color.Transparent, ((float)item.alpha / 255f));
        }
        public override bool OnPickup(Player player)
		{
			player.GetModPlayer<AccursedBladePlayer>().charge += 0.15f;
			Main.PlaySound(SoundID.NPCKilled, (int)player.position.X, (int)player.position.Y, 52, 1f, -0.9f);
			return false;
		}
	}
    internal class AccursedBladePlayer : ModPlayer
	{
		public float charge = 0;
        public override void ResetEffects()
		{
            if (charge > 1)
            {
                charge = 1;
            }
            if (charge > 0)
                charge -= 0.00025f;
            else
                charge = 0;
        }
    }
     public class AccursedBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Accursed Bolt");
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            //Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = 0;
            projectile.scale = 1f;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.penetrate = 2;
            projectile.timeLeft = 270;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.damage = 15;
        }

        //bool primsCreated = false;
        public override void AI()
        {
            //Player player = Main.player[projectile.owner];
            /*if (!primsCreated)
            {
                primsCreated = true;
                Starjinx.primitives.CreateTrail(new SkullPrimTrail(projectile, Color.Green, 10 + (int)(projectile.ai[0] * 15)));
                projectile.penetrate = (int)(projectile.ai[0] * 5) + 1;
            }*/
            projectile.ai[0] += 0.01f;
            projectile.rotation = projectile.velocity.ToRotation();

            if (projectile.timeLeft < 25 || projectile.penetrate <= 1)
                Fadeout();

            if(projectile.ai[1] > 0)
            {
                projectile.velocity *= 0.9f;
                projectile.alpha += 10;
                if (projectile.alpha > 255)
                    projectile.Kill();
            }
        }

        private void Fadeout()
        {
            projectile.ai[1]++;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Fadeout();
            projectile.velocity = oldVelocity;
            return false;
        }
        public override bool CanDamage() => projectile.ai[1] == 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            #region shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector4 colorMod = new Color(142, 223, 38, projectile.alpha).ToVector4();
            SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/vnoise"));
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.25f);
			SpiritMod.StarjinxNoise.Parameters["counter"].SetValue(projectile.ai[0]);
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[2].Apply();
            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition), null, new Color(142, 223, 38) * projectile.Opacity, projectile.rotation, new Vector2(50, 50), projectile.scale * new Vector2(4f, 1) / 2, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion
            return false;
        }
    }
}