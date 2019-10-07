//using UnityEngine;
using Activ.GOAP;

public class BakerAI : GameAI<Baker>, Baker.AI{

    int   temperature = 0;
    float bake;

    override protected Goal<Baker> Goal()
    => new Goal<Baker>( x => x.state == Baker.Cooking.Cooked );

    override protected Baker Model()
    => new Baker(this){ temperature=temperature, bake=bake };

    public void SetTemperature(int degrees)
    => temperature = degrees;

    public void Bake()
    => bake += temperature/2;

}
