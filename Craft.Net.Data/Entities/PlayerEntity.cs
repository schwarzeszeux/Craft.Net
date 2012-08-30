using System;
using Craft.Net.Data.Events;
namespace Craft.Net.Data.Entities
{
    public class PlayerEntity : Entity
    {
        #region Constants

        public const int InventoryHotbar = 36;
        public const int InventoryCraftingGrid = 1;
        public const int InventoryCraftingOutput = 0;
        public const int InventoryArmor = 5;
        public const int InventoryMain = 9;

        #endregion

        #region Properties

        public override Size Size
        {
            get { return new Size(0.6, 1.8, 0.6); }
        }

        public static double Width
        {
            get { return 0.6; }
        }

        public static double Height
        {
            get { return 1.8; }
        }

        public static double Depth
        {
            get { return 0.6; }
        }

        #endregion

        #region Fields

        public string Username;
        /// <summary>
        /// The client's current inventory.
        /// </summary>
        public Slot[] Inventory;
        public short SelectedSlot;

        public GameMode GameMode;

        #endregion

        public PlayerEntity()
        {
            Inventory = new Slot[45];
            for (int i = 0; i < Inventory.Length; i++)
                Inventory[i] = new Slot();
            SelectedSlot = InventoryHotbar;
        }

        public void SetSlot(short index, Slot slot)
        {
            if (InventoryChanged != null)
                InventoryChanged(this, new InventoryChangedEventArgs()
                {
                    Index = index,
                    OldValue = Inventory[index],
                    NewValue = slot
                });
            Inventory[index] = slot;
        }

        public event EventHandler<InventoryChangedEventArgs> InventoryChanged;
    }
}