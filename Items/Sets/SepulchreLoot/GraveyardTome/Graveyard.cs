using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.GraveyardTome
{
	public class Graveyard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Graveyard");
			Tooltip.SetDefault("Creates a portal to the nether realm, releasing a torrent of skulls");
		}

		public override void SetDefaults()
		{
			Item.Size = new Vector2(36, 54);
			Item.damage = 60;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<GraveyardPortal>();
			Item.shootSpeed = 18f;
			Item.knockBack = 3f;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item104;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.mana = 6;
			Item.noUseGraphic = true;
			Item.channel = true;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Projectile.NewProjectile(source, player.Center - new Vector2(50 * player.direction, 50), Vector2.Zero, Item.shoot, damage, knockback, player.whoAmI, player.direction);
			return false;
		}

		public override void HoldItem(Player player)
		{
			GraveyardPlayer modplayer = player.GetModPlayer<GraveyardPlayer>();
			if (player.itemAnimation > 0 && player.channel)
			{
				const int FPS = 7;

				modplayer.GraveyardFrame += (100 / 60f) * (FPS / 60f);
				if (modplayer.GraveyardFrame > 5 && !Main.dedServ)
				{
					modplayer.GraveyardFrame = 0;
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/PageFlip") with { PitchVariance = 0.3f, Volume = 0.75f }, player.Center);
				}

				if (Main.rand.NextBool(10))
				{
					Vector2 velocity = -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(1f, 2f);
					Particles.ParticleHandler.SpawnParticle(new GraveyardRunes(player, Vector2.Zero, velocity, Main.rand.NextFloat(0.6f, 0.8f), 25));
				}
			}
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 2);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<ScreamingTome.ScreamingTome>(), 1);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.Bone, 40);
			recipe.AddIngredient(ItemID.DarkShard, 2);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}
	}
}