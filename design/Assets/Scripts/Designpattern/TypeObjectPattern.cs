//类型对象模式
using UnityEngine;
public class Breed
{
    int health_;
    string attack_;
    Breed parent_;
    public Breed(Breed parent, int health, string attack)
    {
        health_ = health;
        attack_ = attack;
        parent_ = null;

        if (parent != null)
        {
            parent_ = parent;
            if (health_ == 0)
            {
                health_ = health;
            }
            if (string.IsNullOrEmpty(attack_))
            {
                attack_ = attack;
            }
        }
    }
    public int GetHealth()
    {
        return health_;
    }

    public string GetAttack()
    {
        return attack_;
    }
    public Monster NewMonster()
    {
        return new Monster(this);
    }
}
public class Monster
{
    private int health_;
    private Breed breed_;
    private string attack_;
    public Monster(Breed breed)
    {
        health_ = breed.GetHealth();
        breed_ = breed;
        attack_ = breed.GetAttack();
    }
    public string GetAttack()
    {
        return attack_;
    }
    public int GetHealth()
    {
        return health_;
    }
    public void ShowAttack()
    {
        Debug.Log(attack_);
    }

}