using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.ToxicBottle
{
	public class ToxicBottle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Bottle");
			Tooltip.SetDefault("Can be placed or thrown\nExplodes into cursed gas upon being struck");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 0;
			item.value = Terraria.Item.sellPrice(0, 1, 10, 0);
			item.rare = ItemRarityID.Green;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("ToxicBottleProj");
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.noMelee = true;
			item.ranged = true;
			item.damage = 15;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0;
		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				for (int projFinder = 0; projFinder < Main.projectile.Length; ++projFinder)
					if (Main.projectile[projFinder].type == type && Main.projectile[projFinder].active && Main.projectile[projFinder].owner == player.whoAmI)
						Main.projectile[projFinder].Kill();

				return false;
			}
			
			speedY /= 2;
			return true;
		}
	}
	public class ToxicBottleProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Bottle");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 34;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 200;
			projectile.ai[0] = 6;
		}
		ref float JumpCounter => ref projectile.ai[0];
		bool stopped = false;

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(stopped);

		public override void ReceiveExtraAI(BinaryReader reader) => stopped = reader.ReadBoolean();

		public override void AI()
		{
			JumpCounter++;
			projectile.velocity.Y += 0.4F;
			projectile.velocity.X *= 0.98F;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.X != projectile.velocity.X) if (JumpCounter > 5 && !stopped)
            {
                JumpCounter = 0;
                projectile.position.Y -= 10;
                projectile.velocity.X = oldVelocity.X;
            }
            else if (!stopped)
            {
                stopped = true;
                projectile.velocity.X = 0;
                projectile.netUpdate = true;
            }
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
		
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
			for (int num424 = 0; num424 < 30; num424++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 13, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
				Main.dust[dust].scale = .8f;
				int dust1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 61, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
				Main.dust[dust1].scale = .8f;
				int dust12 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 54, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
				Main.dust[dust12].scale = .8f;

				if(timeLeft > 0)
					Projectile.NewProjectile(projectile.position, Vector2.Zero, ModContent.ProjectileType<ToxicBottleField>(), 0, 0, projectile.owner);
			}
		}
	}

	public class ToxicBottleField : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Bottle");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 150;
			projectile.height = 150;
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 150;
			projectile.hide = true;
			projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			for (int i = 50; i < 200 - projectile.timeLeft; i+= 60)
			{
				Vector2 direction = Main.rand.NextFloat(6.28f).ToRotationVector2();
				direction *= Main.rand.Next(i + 1);
				if (projectile.timeLeft > 50 && Main.rand.Next(3) == 1)
				{
					Dust dust = Dust.NewDustPerfect(projectile.Center + direction, ModContent.DustType<PoisonGas>());
					dust.scale = 3;
				}
			}

			var list = Main.npc.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var npc in list.Where(npc => !npc.friendly && Main.rand.Next(30) == 1))
				npc.AddBuff(BuffID.CursedInferno, 15);
		}
	}
}