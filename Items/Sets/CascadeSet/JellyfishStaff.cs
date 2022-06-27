using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.CascadeSet
{
	public class JellyfishStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jellyfish Staff");
			Tooltip.SetDefault("Summons a tiny jellyfish to fight for you!");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/CascadeSet/JellyfishStaff_Glow");
        }

        public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Blue;
			Item.mana = 10;
			Item.damage = 11;
			Item.knockBack = 2.5f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<JellyfishMinion>();
            Item.buffType = ModContent.BuffType<JellyfishMinionBuff>();
            Item.UseSound = SoundID.Item44;
        }
		
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
			//recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 6);
			recipe.AddIngredient(ItemID.Coral, 5);
            recipe.AddIngredient(ItemID.Glowstick, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

		public override bool AltFunctionUse(Player player) => true;

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
				player.MinionNPCTargetAim(true);
			return null;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => position = Main.MouseWorld;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			player.AddBuff(Item.buffType, 2);
			return player.altFunctionUse != 2;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, .224f, .133f, .255f);
			Texture2D texture = TextureAssets.Item[Item.type].Value;

			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("Items/Sets/CascadeSet/JellyfishStaff_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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
	}
}