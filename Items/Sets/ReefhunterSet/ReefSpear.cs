using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class ReefSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reef Trident");
			Tooltip.SetDefault("Right click to throw the trident, poisoning enemies for a short time");
		}

		public override void SetDefaults()
		{
			item.damage = 18;
			item.width = 28;
			item.height = 14;
			item.useTime = item.useAnimation = 30;
			item.knockBack = 2f;
			item.shootSpeed = 0f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.melee = true;
			item.channel = false;
			item.autoReuse = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(gold: 2);
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<ReefSpearProjectile>();
			item.useStyle = ItemUseStyleID.HoldingOut;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);
		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			//Commenting out (unintended?)weapon class changes: classes here seem odd(ranged for melee spear), weapon is already constantly melee, and iirc class changes like this are super jank with prefixes and the like
			if (player.altFunctionUse == 2)
			{
				item.shoot = ModContent.ProjectileType<ReefSpearThrown>();
				//item.thrown = true; 
				//item.ranged = false;
				item.damage = 24;
				item.shootSpeed = 12f;
				item.channel = false;
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 35;

			}
			else
			{
				item.shoot = ModContent.ProjectileType<ReefSpearProjectile>();
				//item.thrown = false;
				//item.ranged = true;
				item.damage = 18;
				item.shootSpeed = 0f;
				item.channel = true;
				item.useTime = item.useAnimation = 40;
				item.useStyle = ItemUseStyleID.HoldingOut;
			}
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				position -= new Vector2(20 * player.direction, 0);

				Vector2 velocity = CalcSpearVel(position);
				speedX = velocity.X;
				speedY = velocity.Y;
			}

			return true;
		}

		/// <summary>
		/// Hardcoded calculation in an attempt to accurately make the spear projectile hit the cursor, only going above the target if necessary to reach it with a capped X velocity
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		private Vector2 CalcSpearVel(Vector2 position)
		{
			Vector2 target = Main.MouseWorld;
			const float minDist = 100;
			if (Vector2.Distance(target, position) < minDist) //If there's an extremely small amount of distance between the firing position and the target, move the target away
				target = position + Vector2.Normalize(target - position) * minDist;

			Vector2 velocity;
			int tries = 0;
			const int maxTries = 10;
			//Adjust arc height to be higher only if it wouldn't be able to reach target with capped x velocity (probably adjust arc velocity calc later to do something like this rather than have this here)
			do
			{
				float heightAboveTarget = tries * 20f ; //Step of 20 pixels per try
				if (target.Y > position.Y && tries > 0) //If target is below the firing position, and if a repeat has already been necessary, move the top of the arc to be above the firing position
					heightAboveTarget += (target.Y - position.Y);

				float? cappedSpeed = null;
				if (tries == maxTries - 1) //If on last try, force velocity to be capped
					cappedSpeed = item.shootSpeed;

				velocity = ArcVelocityHelper.GetArcVel(position, target, 0.3f, null, 400, cappedSpeed, heightAboveTarget, item.shootSpeed / 3);
				tries++;
			} while (System.Math.Abs(velocity.X) > item.shootSpeed && tries < maxTries); //Repeat if velocity is too high 

			return velocity;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 10);
			recipe.AddIngredient(ModContent.ItemType<SulfurDeposit>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}