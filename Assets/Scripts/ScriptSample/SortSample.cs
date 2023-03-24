using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AddAttackGoal(0);
        AddAttackGoal(10);
        AddAttackGoal(20);
        AddAttackGoal(15);
        AddAttackGoal(5);
        AddAttackGoal(10);
        foreach (var item in attackGoals)
        {
            Debug.Log(item.priority);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<AttackGoal> attackGoals = new List<AttackGoal>();
    public void AddAttackGoal(int priority = 0)
    {
        attackGoals.Add(new AttackGoal(priority));
        attackGoals.Sort((x, y) => x.Compare(x, y));
    }
    public class AttackGoal : Comparer<AttackGoal>
    {
        public float attackDis;
        public float duration;
        public int priority;
        public float durationTimer;


        public AttackGoal(int priority)
        {
            this.priority = priority;
        }

        public override int Compare(AttackGoal x, AttackGoal y)
        {
            if (x.priority > y.priority)
            {
                return 1;
            }
            else if (x.priority == y.priority)
            {
                return 0;
            }
            return -1;
        }
    }
}
