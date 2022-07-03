using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.BackgroundSystem;
using SpiritMod.Mechanics.BackgroundSystem.BGItem;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Archon.Projectiles
{
	class BGStarProjectile : ModProjectile
	{
		public float Z = 0.01f;

		private StarProjBGItem Background = null;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			DisplayName.SetDefault("Fleeing Star");
		}

		public override void SetDefaults()
		{
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
			Projectile.tileCollide = true;
			Projectile.timeLeft = 150;
		}

		public override void AI()
		{
			if (Background == null)
			{
				Background = new StarProjBGItem(Projectile.Center, Projectile);
				BackgroundItemManager.AddItem(Background);
			}

			Projectile.position = Background.Center;

			Z *= 1.15f;

			if (Z > 6.2f)
				Projectile.Kill();
		}

		public override bool CanHitPlayer(Player target) => Z >= 1f;

		public override bool PreDraw(ref Color lightColor)
		{
			if (Z < 0.5f)
				return false;

			Projectile.scale *= 1.05f;
			//projectile.width = (int)(22 * projectile.scale);
			//projectile.height = (int)(22 * projectile.scale);
			return true;
		}
	}

	class StarProjBGItem : BaseBGItem
	{
		private readonly Projectile Parent;
		private BGStarProjectile ModParent => Parent.ModProjectile as BGStarProjectile;

		public StarProjBGItem(Vector2 pos, Projectile parent) : base(pos, 0f, new Point(0, 0))
		{
			tex = ModContent.Request<Texture2D>("SpiritMod/NPCs/StarjinxEvent/Enemies/Archon/Projectiles/BGStarProjectile", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			source = new Rectangle(0, 0, tex.Width, tex.Height);
			scale = 1f;

			Parent = parent;
		}

		internal override void Behaviour()
		{
			if (Parent == null || ModParent == null)
				return;
			//Center = Parent.Center;
			rotation = Parent.rotation;

			scale = ModParent.Z * 8;
			BaseParallax(.01f);

			if (ModParent.Z >= 0.49f)
			{
				Parent.scale = scale;
				killMe = true;
			}
		}

		internal override void Draw(Vector2 off)
		{
			if (ModParent == null || Parent == null || ModParent.Z >= 0.5f)
				return;

			drawColor = Color.Lerp(Main.ColorOfTheSkies, Color.White, 0.5f);

			if (ModParent.Z < 0.2f)
				drawColor *= ModParent.Z * 5;
			base.Draw(GetParallax());
		}
	}
}
