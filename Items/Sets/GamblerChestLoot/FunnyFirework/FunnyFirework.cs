using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GamblerChestLoot.FunnyFirework
{
	public class FunnyFirework : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Funny Firework");
			Tooltip.SetDefault("Does a funny");
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 40;
			Item.useTime = 61;
			Item.useAnimation = 61;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 0;
			Item.damage = 4;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<FunnyFireworkProj>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.consumable = true;
			Item.maxStack = 999;
		}
	}

	public class FunnyFireworkProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Funny Firework");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 90;
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
		}
		
		public override void Kill(int timeLeft)
		{
			switch (Main.rand.Next(4))
			{
				case 0:
					DustHelper.DrawDustImageRainbow(Projectile.Center, 0.125f, "SpiritMod/Effects/DustImages/GarfieldFirework", 1f);
					break;
				case 1:
					DustHelper.DrawDustImageRainbow(Projectile.Center, 0.125f, "SpiritMod/Effects/DustImages/GladeFirework", 1f);
					break;
				case 2:
					DustHelper.DrawDustImageRainbow(Projectile.Center, 0.125f, "SpiritMod/Effects/DustImages/TrollFirework", 1f);
					break;
				case 3:
					DustHelper.DrawDustImageRainbow(Projectile.Center, 0.125f, "SpiritMod/Effects/DustImages/AmogusFirework", 1f);
					break;
			}
			SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
		}
	}
}