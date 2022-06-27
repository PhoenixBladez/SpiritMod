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
			Item.damage = 18;
			Item.width = 28;
			Item.height = 14;
			Item.useTime = Item.useAnimation = 30;
			Item.knockBack = 2f;
			Item.shootSpeed = 0f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Melee;
			Item.channel = false;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(gold: 2);
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<ReefSpearProjectile>();
			Item.useStyle = ItemUseStyleID.Shoot;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);
		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			//Commenting out (unintended?)weapon class changes: classes here seem odd(ranged for melee spear), weapon is already constantly melee, and iirc class changes like this are super jank with prefixes and the like
			if (player.altFunctionUse == 2)
			{
				Item.shoot = ModContent.ProjectileType<ReefSpearThrown>();
				//Item.DamageType = DamageClass.Throwing; 
				//item.ranged = false;
				Item.damage = 24;
				Item.shootSpeed = 12f;
				Item.channel = false;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.useTime = Item.useAnimation = 35;

			}
			else
			{
				Item.shoot = ModContent.ProjectileType<ReefSpearProjectile>();
				//item.thrown = false;
				//Item.DamageType = DamageClass.Ranged;
				Item.damage = 18;
				Item.shootSpeed = 0f;
				Item.channel = true;
				Item.useTime = Item.useAnimation = 40;
				Item.useStyle = ItemUseStyleID.Shoot;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				position -= new Vector2(20 * player.direction, 0);
				velocity = CalcSpearVel(position);
			}
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
					cappedSpeed = Item.shootSpeed;

				velocity = ArcVelocityHelper.GetArcVel(position, target, 0.3f, null, 400, cappedSpeed, heightAboveTarget, Item.shootSpeed / 3);
				tries++;
			} while (System.Math.Abs(velocity.X) > Item.shootSpeed && tries < maxTries); //Repeat if velocity is too high 

			return velocity;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 10);
			recipe.AddIngredient(ModContent.ItemType<SulfurDeposit>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}