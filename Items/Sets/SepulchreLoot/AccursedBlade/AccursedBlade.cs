using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.UI.ChargeMeter;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.AccursedBlade
{
    public class AccursedBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Accursed Blade");
            Tooltip.SetDefault("Kill enemies and collect their souls to build up charge\nRight click to release charge as a cursed bolt");
            SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f; 
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.damage = 19;
            Item.width = 30;
            Item.height = 30;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<AccursedBolt>();
            Item.shootSpeed = 9;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            GlowmaskUtils.DrawItemGlowMaskWorld(Main.spriteBatch, Item, Mod.Assets.Request<Texture2D>(Texture.Remove(0, Mod.Name.Length + 1) + "_glow").Value, rotation, scale);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if(player.GetModPlayer<AccursedBladePlayer>().charge == 0)
                    return false;

                Item.useStyle = ItemUseStyleID.Shoot;
                Item.staff[Item.type] = true;
                Item.noMelee = true;
                return true;
            }
            Item.useStyle = ItemUseStyleID.Swing;
            Item.staff[Item.type] = false;
            Item.noMelee = false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
            if (player.altFunctionUse != 2 || player.GetModPlayer<AccursedBladePlayer>().charge == 0)
            {
                return false;
            }
            damage = 10 + (int)(player.GetModPlayer<AccursedBladePlayer>().charge * 25);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.GetModPlayer<AccursedBladePlayer>().charge);
            player.GetModPlayer<AccursedBladePlayer>().charge = 0;
            SoundEngine.PlaySound(SoundID.NPCDeath52 with { Pitch = 1.2f }, player.position);
            return true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
               Item.NewItem(Item.GetSource_OnHit(target), (int)target.position.X, (int)target.position.Y - 20, target.width, target.height, Mod.Find<ModItem>("AccursedSoul").Type);
        }
    }
    public class AccursedSoul: ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Accursed Soul");
			Tooltip.SetDefault("You shouldn't see this");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
		}

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 20;
			Item.maxStack = 1;
			Item.alpha = 0;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
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
            Item.alpha++;
            if (Item.alpha > 255)
                Item.active = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.White, Color.Transparent, ((float)Item.alpha / 255f));
        }
        public override bool OnPickup(Player player)
		{
			player.GetModPlayer<AccursedBladePlayer>().charge += 0.15f;
			SoundEngine.PlaySound(SoundID.NPCKilled, (int)player.position.X, (int)player.position.Y, 52, 1f, -0.9f);
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
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 270;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.damage = 15;
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
            Projectile.ai[0] += 0.01f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.timeLeft > 250)
            {
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.timeLeft < 25 || Projectile.penetrate <= 1)
                Fadeout();

            if(Projectile.ai[1] > 0)
            {
                Projectile.velocity *= 0.9f;
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                    Projectile.Kill();
            }
        }

        private void Fadeout()
        {
            Projectile.ai[1]++;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Fadeout();
            Projectile.velocity = oldVelocity;
            return false;
        }
        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Projectile.ai[1] == 0;

        public override bool PreDraw(ref Color lightColor)
        {
            #region shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector4 colorMod = new Color(142, 223, 38, Projectile.alpha).ToVector4();
            SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("Textures/vnoise").Value);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.25f);
			SpiritMod.StarjinxNoise.Parameters["counter"].SetValue(Projectile.ai[0]);
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[2].Apply();
            Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value, (Projectile.Center - Main.screenPosition), null, new Color(142, 223, 38) * Projectile.Opacity, Projectile.rotation, new Vector2(50, 50), Projectile.scale * new Vector2(4f, 1) / 2, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion
            return false;
        }
    }
}