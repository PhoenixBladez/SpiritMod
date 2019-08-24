using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class GloomgusStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glumshroom Staff");
			Tooltip.SetDefault("Summons a explosive mushroom");
		}


		public override void SetDefaults()
		{
		    item.CloneDefaults(ItemID.QueenSpiderStaff); //only here for values we haven't defined ourselves yet
			item.damage = 51;  //placeholder damage :3
			item.mana = 10;   //somehow I think this might be too much...? -thegamemaster1234
			item.width = 40;
			item.height = 40;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
			item.useTime = 20;
			item.useAnimation = 20;
			item.autoReuse = true;
            item.knockBack = 2.5f;
			item.UseSound = SoundID.Item25;
			item.shoot = mod.ProjectileType("GloomgusShroom");
			item.shootSpeed = 0f;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			
            //projectile spawns at mouse cursor
            Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = value18;
            return true;
        }
	}
}