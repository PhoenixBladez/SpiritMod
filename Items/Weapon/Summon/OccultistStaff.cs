using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class OccultistStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Occultist's Swarm");
			Tooltip.SetDefault("Summons hordes of Blood Zombies\nBlood Zombies may restore health upon hitting enemies\nUp to 4 zombies may exist at once\nZombies take up 1/3 of a minion slot");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Summon/OccultistStaff_Glow");
		}


		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.sellPrice(0, 1, 27, 46);
			item.rare = ItemRarityID.Blue;
			item.mana = 10;
			item.damage = 12;
			item.knockBack = 2;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.melee = false;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.shoot = ModContent.ProjectileType<ZombieMinion>();
			item.UseSound = SoundID.Item44;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.46f, .07f, .52f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Summon/OccultistStaff_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
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
		public override bool UseItem(Player player)
		{
			if(player.altFunctionUse == 2) {
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			//projectile spawns at mouse cursor
			Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
			position = value18;
			for(int i = 0; i <= Main.rand.Next(1, 2); i++) {
				Terraria.Projectile.NewProjectile(position.X + Main.rand.Next(-30, 30), position.Y, 0f, 0f, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}