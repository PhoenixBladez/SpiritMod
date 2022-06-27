using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.GraniteSet;
using SpiritMod.Projectiles.Summon.Zones;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs.Zones;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.ArcaneZoneSubclass
{
	public class RepulsionCodex : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Codex: Repulsion Zone");
			Tooltip.SetDefault("Summons a repulsion zone at the cursor position\nRepulsion zones push nearby enemies away\nRepulsion zones break after multiple enemy strikes\nOnly one repulsion zone can exist at once\nZones count as sentries");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/ArcaneZoneSubclass/RepulsionCodex_Glow");
        }

        public override void SetDefaults()
		{
			Item.damage = 0;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 54;
			Item.height = 50;
			Item.useTime = 31;
			Item.useAnimation = 31;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.reuseDelay = 60;
			Item.UseSound = SoundID.DD2_EtherianPortalSpawnEnemy;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<RepulsionZone>();
			Item.shootSpeed = 0f;
			Item.buffType = ModContent.BuffType<RepulsionZoneTimer>();
			Item.buffTime = Projectile.SentryLifeTime;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			position = Main.MouseWorld;
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            player.UpdateMaxTurrets();
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.133f, .031f, .211f);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.Assets.Request<Texture2D>("Items/Sets/ArcaneZoneSubclass/RepulsionCodex_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EmptyCodex>(), 1);
            recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}