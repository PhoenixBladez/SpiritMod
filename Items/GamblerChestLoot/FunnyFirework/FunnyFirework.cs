using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.GamblerChestLoot.FunnyFirework
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
			item.width = 44;
			item.height = 40;
			item.useTime = 61;
			item.useAnimation = 61;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 0;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("FunnyFireworkProj");
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.consumable = true;
			item.maxStack = 999;
		}
	}
	public class FunnyFireworkProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Funny Firework");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 50;
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
		}
		
		public override void Kill(int timeLeft)
		{
			switch (Main.rand.Next(4))
			{
				case 0:
					DustHelper.DrawDustImageRainbow(projectile.Center, 0.125f, "SpiritMod/Effects/DustImages/GarfieldFirework", 1f);
					break;
				case 1:
					DustHelper.DrawDustImageRainbow(projectile.Center, 0.125f, "SpiritMod/Effects/DustImages/GladeFirework", 1f);
					break;
				case 2:
					DustHelper.DrawDustImageRainbow(projectile.Center, 0.125f, "SpiritMod/Effects/DustImages/TrollFirework", 1f);
					break;
				case 3:
					DustHelper.DrawDustImageRainbow(projectile.Center, 0.125f, "SpiritMod/Effects/DustImages/AmogusFirework", 1f);
					break;
			}
			Main.PlaySound(SoundID.Item14, projectile.Center);
		}
	}
}