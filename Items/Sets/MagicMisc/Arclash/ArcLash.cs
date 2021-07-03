using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;
using System;
namespace SpiritMod.Items.Sets.MagicMisc.Arclash
{
	public class ArcLash : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arc Lash");
			Tooltip.SetDefault("Does more damage towards the end of its cycle");
		}
		public override void SetDefaults()
		{
			item.damage = 12;
			item.magic = true;
			item.mana = 5;
			item.width = 44;
			item.height = 46;
			item.channel = true;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 1, 10, 0);
			item.UseSound = SoundID.Item15;
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<ArcLashProj>();
			item.shootSpeed = 8;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 direction = new Vector2(speedX,speedY);
			position+= direction * 7;
			Projectile.NewProjectile(position, Vector2.Zero, type, damage, knockBack, player.whoAmI);
			return false;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0;
	}
	public class ArcLashProj : ModProjectile
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Arc Lash");

		private readonly int cycletime = 40;
		private readonly float lengthmult = 50;
		private ref float Progress => ref projectile.ai[0];

		private ref float Angle => ref projectile.ai[1];

		Player Owner => Main.player[projectile.owner];

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.aiStyle = -1;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = cycletime;
            projectile.alpha = 255;
        }

        public override void AI()
        {
			if (!Owner.channel)
				projectile.Kill();

			if (Main.LocalPlayer == Owner) {
				Angle = Owner.AngleTo(Main.MouseWorld);
				projectile.netUpdate = true;
			}

			Progress = lengthmult * (cycletime - projectile.timeLeft)/cycletime;
			Owner.ChangeDir(Math.Sign(projectile.Center.X - Owner.MountedCenter.X));
			Owner.itemRotation = MathHelper.WrapAngle(Angle - Owner.fullRotation - ((Owner.direction < 0) ? MathHelper.Pi : 0));
			Owner.itemTime = 2;
			Owner.itemAnimation = 2;

			if(Progress == cycletime/2) {
				Owner.CheckMana(Owner.HeldItem, Owner.HeldItem.mana, true);
				Main.PlaySound(Owner.HeldItem.UseSound, Owner.MountedCenter);
			}

			Vector2 direction = Vector2.UnitX * 8;
			direction = direction.RotatedBy(projectile.ai[1]);
			Vector2 position = Owner.MountedCenter + (direction * (7 + (Progress / 30)));
			projectile.Center = position + (direction * Progress * 0.66f);

			if (Main.netMode != NetmodeID.Server && ++projectile.localAI[0] == 1) {
				SpiritMod.primitives.CreateTrail(new ArclashPrimTrail(projectile));
			}
        }

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = (int)(Owner.HeldItem.damage * (int)Math.Pow(Progress, 0.3f));
	}
}