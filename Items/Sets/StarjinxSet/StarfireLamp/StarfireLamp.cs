using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SpiritMod.Items.Material;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
    public class StarfireLamp : ModItem
    {
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfire Lantern");
			Tooltip.SetDefault($"Emits embers of cosmic energy\nRight click to illuminate an enemy for {(int)(Item.mana * IlluminateManaRatio)} mana\nEmbers lock on to illuminated enemies");
		}

		private const int baseMana = 6;
		private const int baseIlluminateMana = 50;
		private float IlluminateManaRatio => baseIlluminateMana / (float)baseMana;

		public override void SetDefaults()
        {
            Item.damage = 56;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Magic;
            Item.width = 36;
            Item.height = 40;
			Item.useTime = 7;
            Item.useAnimation = 35;
			Item.reuseDelay = 24;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.shoot = ModContent.ProjectileType<StarfireProj>();
            Item.shootSpeed = 24f;
            Item.knockBack = 3f;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(silver: 55);
            Item.useTurn = false;
            Item.mana = 6;
			Item.holdStyle = 1;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{

			if (player.altFunctionUse == 2)
			{
				Item.useTime = Item.useAnimation;
				Item.reuseDelay = 0;

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
			Item.useTime = Item.useAnimation / 5;
			Item.reuseDelay = 24;

			return true;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult) => mult = (player.altFunctionUse == 2) ? IlluminateManaRatio / (float)baseMana : 1;

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			//Adjust position to account for hold style
			position.Y += 26;
			position.X += 18 * player.direction;

			velocity = Vector2.Normalize(Main.MouseWorld - position).RotatedByRandom(MathHelper.Pi / 20) * Item.shootSpeed;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			//StarfireLampPlayer starfireLampPlayer = player.GetModPlayer<StarfireLampPlayer>(); //NEEDSUPDATING
			//starfireLampPlayer.TwinkleTime = StarfireLampPlayer.MaxTwinkleTime;

			//Done here to be on use rather than on animation start
			if (!Main.dedServ)
				SoundEngine.PlaySound(SoundID.Item45 with { PitchVariance = 0.2f, Volume = 0.5f }, player.Center);

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

				//starfireLampPlayer.LampTargetNPC = mousehovernpc;
				//starfireLampPlayer.LampTargetTime = StarfireLampPlayer.MaxTargetTime; //NEEDSUPDATING
				return false;
			}

			//starfireLampPlayer.GlowmaskOpacity = 1;
            return true;
        }

        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 6);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
