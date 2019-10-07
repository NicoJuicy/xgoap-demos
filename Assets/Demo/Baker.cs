using System;
using System.Collections.Generic;
using UnityEngine;
using Activ.GOAP;
using Action = Activ.GOAP.Action;

[Serializable]
public class Baker : Agent, Parametric{

    public const int Step    = 55;
    public const int MaxHeat = 200;

    public enum Cooking{Raw, Cooked, Burned}

    public float cost { get; set; }

    public int   temperature = 0;
    public float bake;

    public Cooking state => bake < 80  ? Cooking.Raw :
                            bake < 120 ? Cooking.Cooked :
                                         Cooking.Burned;

    [NonSerialized] AI client;

    public Baker(AI client) => this.client = client;

    public bool Bake(){
        cost++;
        bake += (temperature / 2); return true;
    }

    public bool SetTemperature(int degrees){
        cost++;
        temperature = degrees;
        return true;
    }

    Func<bool>[] Agent.actions
    => state != Cooking.Burned ? new Func<bool>[]{ Bake }
                               : null;

    Action[] Parametric.methods
    => state != Cooking.Burned ? CookingOptions() : null;

    Action[] CookingOptions(){
        List<Action> actions = new List<Action>();
        for(int i = 0; i <= MaxHeat; i += Step){
            var j = i;  // Do not capture the iterator!
            actions.Add(new Action(
                () => SetTemperature(j),
                () => client.SetTemperature(j)
            ));
        }
        return actions.ToArray();
    }

    override public bool Equals(object other){
        if(other is Baker that){
            return this.bake == that.bake
                && this.temperature == that.temperature;
        }
        return false;
    }

    override public int GetHashCode()
    => temperature + (int)(bake * 1000);

    override public string ToString()
    => $"Baker[ {state} at {temperature}℃ ]";

    public interface AI{
        void Bake();
        void SetTemperature(int i);
    }

}
