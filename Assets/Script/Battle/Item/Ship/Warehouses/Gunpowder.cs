using System;
using System.Collections.Generic;

public class Gunpowder : Warehouse
{

    public Gunpowder() : base(40, Ship_Item.POWDER)
    { }

    protected override void dealDamageOnDestroy()
    {
        this.getParentShip().receiveDamage(10);
    }

    protected override void applyMalusOnNotWorking()
    {
        this.getParentShip().setPowderAvailable(false);
    }

    protected override void applyChangeOnRevive()
    {
        this.getParentShip().setPowderAvailable(true);
    }
}
