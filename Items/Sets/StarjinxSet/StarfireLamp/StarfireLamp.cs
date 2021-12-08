using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
    public class StarfireLamp : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfire Lantern");
			Tooltip.SetDefault($"Emits embers of cosmic energy\nRight click to illuminate an enemy for {(int)(item.mana * IlluminateManaRatio)} mana\nEmbers lock on to illuminated enemies");
		}

		private const int baseMana = 6;
		private const int baseIlluminateMana = 50;
		private float IlluminateManaRatio => baseIlluminateMana / (float)baseMana;
		public override void SetDefaults()
        {
            item.damage = 56;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.magic = true;
            item.width = 36;
            item.height = 40;
			item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.shoot = ModContent.ProjectileType<StarfireProj>();
            item.shootSpeed = 24f;
            item.knockBack = 3f;
            item.autoReuse = true;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item45.WithPitchVariance(0.2f).WithVolume(0.5f);
            item.value = Item.sellPrice(silver: 55);
            item.useTurn = false;
            item.mana = 6;
        }

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
				item.useTime = 30;
				item.useAnimation = 30;
				NPC mousehovernpc = null; //see if an npc is intersecting the mouse
				foreach (NPC npc in Main.npc.Where(x => x.active && x.CanBeChasedBy(player) && x != null))
				{
					Rectangle hitbox = npc.Hitbox;
					hitbox.Inflate(40, 40);
					if (hitbox.Contains(Main.MouseWorld.ToPoint()))
						mousehovernpc = npc;
				}

				if (mousehovernpc == null)
					return false;

				return true;
			}
			item.useTime = 9;
			item.useAnimation = 9;
			return true;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult) => mult = (player.altFunctionUse == 2) ? IlluminateManaRatio / (float)baseMana : 1;

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			StarfireLampPlayer starfireLampPlayer = player.GetModPlayer<StarfireLampPlayer>();
			starfireLampPlayer.TwinkleTime = StarfireLampPlayer.MaxTwinkleTime;

			if (player.altFunctionUse == 2)
			{
				NPC mousehovernpc = null;

				foreach (NPC npc in Main.npc.Where(x => x.active && x.CanBeChasedBy(player) && x != null)) //iterate through npcs and filter out ones that shouldnt be targetted
				{
					Rectangle hitbox = npc.Hitbox;
					hitbox.Inflate(40, 40);
					if (hitbox.Contains(Main.MouseWorld.ToPoint()))
						mousehovernpc = npc;
				}

				if (mousehovernpc == null)
					return false;

				starfireLampPlayer.LampTargetNPC = mousehovernpc;
				starfireLampPlayer.LampTargetTime = StarfireLampPlayer.MaxTargetTime;
				return false;
			}

			position.Y -= 60;

			Vector2 vel = Vector2.Normalize(Main.MouseWorld - position).RotatedByRandom(MathHelper.Pi / 20) * item.shootSpeed;
			speedX = vel.X;
			speedY = vel.Y;
            return true;
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 6);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
