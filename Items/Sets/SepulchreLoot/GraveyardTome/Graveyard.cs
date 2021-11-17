using Microsoft.Xna.Framework;
using Terraria;
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
			item.Size = new Vector2(36, 54);
            item.damage = 60;
            item.noMelee = true;
            item.magic = true;
            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<GraveyardPortal>();
            item.shootSpeed = 18f;
            item.knockBack = 3f;
            item.autoReuse = true;
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item104;
            item.value = Item.buyPrice(0, 1, 40, 0);
            item.mana = 6;
			item.noUseGraphic = true;
            item.channel = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[item.shoot] > 0)
                return false;

            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(player.Center - new Vector2(50 * player.direction, 50), Vector2.Zero, item.shoot, damage, knockBack, player.whoAmI, player.direction);
            return false;
        }

		public override void HoldItem(Player player)
		{
			GraveyardPlayer modplayer = player.GetModPlayer<GraveyardPlayer>();
			if(player.itemAnimation > 0)
			{
				int framespersecond = 7;
				modplayer.GraveyardFrame += (100 / 60f) * (framespersecond / 60f);
				if (modplayer.GraveyardFrame > 5 && !Main.dedServ)
				{
					modplayer.GraveyardFrame = 0;
					Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/PageFlip").WithPitchVariance(0.2f), player.Center);
				}

				if (Main.rand.NextBool(10))
				{
					Vector2 velocity = -Vector2.UnitY.RotatedByRandom(MathHelper.Pi/4) * Main.rand.NextFloat(1f, 2f);
					Particles.ParticleHandler.SpawnParticle(new GraveyardRunes(player, Vector2.Zero, velocity, Main.rand.NextFloat(0.6f, 0.8f), 25));
				}
			}
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 2);

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ScreamingTome.ScreamingTome>(), 1);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.Bone, 40);
			recipe.AddIngredient(ItemID.DarkShard, 2);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
