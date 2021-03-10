using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;
using System;
namespace SpiritMod.Items.Weapon.Magic.Arclash
{
	public class ArcLash : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arc Lash");
			Tooltip.SetDefault("Does more damage towards the end of it's cycle");
		}

		float arcProgress = 0;
		public override void SetDefaults()
		{
			item.damage = 12;
			item.magic = true;
			item.mana = 1;
			item.width = 44;
			item.height = 46;
			item.channel = true;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 01, 10, 0);
			item.rare = 2;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<ArcLashProj>();
			item.shootSpeed = 8;
		}
		Color color = Color.Green;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			arcProgress++;
			arcProgress *= 1.07f;
			if (arcProgress >= 50)
			{
				int red = Main.rand.Next(60, 255);
				int green = Main.rand.Next(60, 255);
				int blue = Main.rand.Next(60, 255);
				color = new Color(red, green, blue);
			}
			arcProgress%= 50;
			Vector2 direction = new Vector2(speedX,speedY);
			position+= direction * (7 + (arcProgress / 30));
			int proj = Projectile.NewProjectile(position + (direction * arcProgress * 0.66f), Vector2.Zero, type, damage * (int)Math.Pow(arcProgress, 0.3f), knockBack, player.whoAmI);
			if (Main.netMode != NetmodeID.Server)
			{
				SpiritMod.primitives.CreateTrail(new ArclashPrimTrail(Main.projectile[proj], position + (direction.RotatedBy(-0.3f - (arcProgress / 250f)) * arcProgress), position + (direction.RotatedBy(0.3f + (arcProgress / 250f)) * arcProgress), position, position, arcProgress, color));
			}
			return false;
        }
       /* public override void HoldItem(Player player)
		{
			if (!player.channel && player.itemAnimation == 0) arcProgress = 0;
		}*/
	}
	public class ArcLashProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arc Lash");
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.aiStyle = -1;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 5;
            projectile.extraUpdates = 0;
            projectile.alpha = 255;
        }

        public override void AI()
        {
        }
    }
}