using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Enemy : Character
    {
        struct NodeRecord
        {
            public Waypoint currentWaypoint;
            public Waypoint fromWaypoint;
            public NodeRecord[] fromNode;
            public float costSoFar;
            public float estimatedTotalCost;
        }

        public Object target = null;
        protected int teamSize;//max number of attackers on one target
        protected int initialTeamSize = 3;
        protected int teamSizeFinal = int.MaxValue;
        // Removed height as it was not and will not be used
        const float width = 1.5f;
        const float depth = 1.5f;
        // Added mini BB sizes to test with smaller models
        const float miniDepth = .7f;
        const float miniWidth = .7f;
        float attackDistanceLimit;
        const float initialAttackDistanceLimit = 450;
        const float attackDistanceLimitFinal = float.MaxValue;
        // Used for offsetting the spawn position of the enemy so that we can rarely (never?)
        // Have two towers that overlap exactly, causing the game to act unexpectedly
        static Random rand = new Random();

        Waypoint nearestWaypoint, subTarget;
        bool newDestination = true, newTarget = false, chasingPlayer = false;
        int pathStep = 0;
        List<Waypoint> pathToTarget = new List<Waypoint>();
        Waypoint nearestToTarget;

        // Removed position from this constructor as it will be taken from the spawnpoint
        public Enemy(Game game, Model[] modelComponents, SpawnPoint spawnPoint, Weapon[] weapons)
            : base(game, modelComponents, new Vector3(spawnPoint.position.X +(float)rand.NextDouble()/2, 
                                                      spawnPoint.position.Y, 
                                                      spawnPoint.position.Z + (float)rand.NextDouble()/2), 
                   80, 20, 0.08f, spawnPoint, weapons, new FloatRectangle(spawnPoint.position.X, spawnPoint.position.Z, miniWidth, miniDepth), true)
        {
            nearestToTarget = new Waypoint(this.game, Vector3.Zero);
            teamSize = initialTeamSize;
            attackDistanceLimit = initialAttackDistanceLimit;
        }

        protected void chooseTarget2(List<Building> buildings, List<Player> players, List<Tower> towers) 
        {
            float distance = float.PositiveInfinity;
            foreach (Building b in buildings)
            {
                if ((b.position - this.position).Length() < distance)
                {
                    if (b is Generator)
                    {
                        Generator g = (Generator)b;
                        if (g.hp <= 50 && !g.functional) 
                            continue;
                        distance = (b.position - this.position).Length();
                        target = b;
                    }
                }
            }
            foreach (Player p in players)
            {
                if (p.attackerNum < teamSize && (p.position - this.position).Length() < distance)
                {
                    distance = (p.position - this.position).Length();
                    target = p;
                    newTarget = true;
                }
            }
            foreach (Tower t in towers)
            {
                if (t.attackerNum < teamSize && (t.position - this.position).Length() < distance)
                {
                    distance = (t.position - this.position).Length();
                    target = t;
                }
            }
            if (distance > attackDistanceLimit)
            {
                target = null;
            }
            else if (target is Player) { 
                Player p = (Player)target;
                ++p.attackerNum;
            }
            else if (target is Tower)
            {
                Tower t = (Tower)target;
                ++t.attackerNum;
            }
        }

        public void Update2(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget, List<Waypoint> waypointsList)
        {
            if (target != null)
            {
                if (target is Generator)
                {
                    Generator g = (Generator)target;
                    if (!g.functional)
                    {
                        target = null;
                        velocity = Vector3.Zero;
                    }
                }
                else if (target is Player)
                {
                    if (newTarget)
                    {
                        newDestination = true;
                        newTarget = false;
                    }
                    if (newDestination)
                    {
                        pathStep = 0;
                        pathToTarget = aStarToTarget(waypointsList);
                        newDestination = false;
                        subTarget = pathToTarget[0];
                    }
                    //if (aiCounter >= 1)
                    //{
                    //    if ((subTarget.position - this.position).LengthSquared() < 1)
                    //    {
                    //        newDestination = true;
                    //        aiCounter = 0;
                    //    }
                    //}
                    else if (subTarget != null)
                    {
                        if ((subTarget.position - this.position).LengthSquared() < 1)
                        {
                            if (pathStep < pathToTarget.Count)
                                subTarget = pathToTarget[pathStep++];
                            else newDestination = true;
                        }
                        velocity = subTarget.position - this.position;
                        velocity.Normalize();
                    }
                    if ((target.position - this.position).Length() > attackDistanceLimit)
                    {
                        Player p = (Player)target;
                        --p.attackerNum;
                        target = null;
                        velocity = Vector3.Zero;
                        pathStep = 0;
                    }
                }
                /*
                float targetDist = (target.position - this.position).Length();
                if (targetDist < weapons[selectedWeapon].range)
                {
                    velocity = Vector3.Zero;
                    //weapons[selectedWeapon].shoot(this.position, lookDirection, true, gameTime, cameraTarget);
                    lookDirection = target.position - this.position;
                }
                else
                {
                    //collisionAvoidance(colliders);
                    lookDirection = velocity;
                    if (velocity.Length() == 0)
                    {
                        velocity = target.position - this.position;
                        lookDirection = velocity;
                    }
                }
                */
            }
            if (target == null || !target.isAlive)
            {
                List<Building> buildings = new List<Building>();
                List<Player> players = new List<Player>();
                List<Tower> towers = new List<Tower>();

                foreach (GameComponent gc in game.Components)
                {
                    if (gc is Player)
                    {
                        players.Add((Player)gc);
                    }
                    if (gc is Map)
                    {
                        Map m = (Map)gc;
                        buildings = m.usableBuildings;
                    }
                    if (gc is Tower)
                    {
                        towers.Add((Tower)gc);
                    }
                }

                chooseTarget(buildings, players, towers);
            }
            /*
            else if (target != null)//start attacking the target the Update chooses
            {
                float targetDist = (target.position - this.position).Length();
                if (targetDist < weapons[selectedWeapon].range)
                {
                    velocity = Vector3.Zero;
                    //weapons[selectedWeapon].shoot(this.position, lookDirection, true, gameTime, cameraTarget);
                    lookDirection = target.position - this.position;
                }
                else
                {
                    //collisionAvoidance(colliders);
                    lookDirection = velocity;
                    if (velocity.Length() == 0)
                    {
                        velocity = target.position - this.position;
                        lookDirection = velocity;
                    }
                }
            }
            */
            //bounds are updated in Character
            //this.bounds = new FloatSphere(position.X, position.Z, width, depth);
            weapons[selectedWeapon].Update(gameTime, colliders, cameraTarget);
            base.Update(gameTime, colliders, cameraTarget, waypointsList);
        }

        private void collisionAvoidance2(List<Object> colliders)
        {
            Vector3 currentDestination = target.position - this.position;

            // If there is something directly in front of you
            if (needToSteer(colliders, currentDestination))
            {
                float angle = 12;

                Vector3 right = Vector3.Zero;
                Vector3 left = Vector3.Zero;

                right.X = (velocity.X * (float)Math.Cos(MathHelper.ToRadians(angle))) - (velocity.Z * (float)Math.Sin(MathHelper.ToRadians(angle)));
                right.Z = (velocity.X * (float)Math.Sin(MathHelper.ToRadians(angle))) + (velocity.Z * (float)Math.Cos(MathHelper.ToRadians(angle)));

                left.X = (velocity.X * (float)Math.Cos(MathHelper.ToRadians(-angle))) - (velocity.Z * (float)Math.Sin(MathHelper.ToRadians(-angle)));
                left.Z = (velocity.X * (float)Math.Sin(MathHelper.ToRadians(-angle))) + (velocity.Z * (float)Math.Cos(MathHelper.ToRadians(-angle)));

                if ((this.position + right - target.position).Length() > (this.position + left - target.position).Length())
                {
                    velocity = right;
                }
                else 
                {
                    velocity = left;
                }
            }
            else// if (!needToSteer(colliders, velocity))  //current direction is good, keep going
            {}
        }

        private bool needToSteer(List<Object> colliders, Vector3 direction)
        {
            Vector3 step = direction;
            step.Normalize();
            FloatRectangle next;
            foreach (Object o in colliders) 
            {
                if (!(o is Bullet))
                {
                    //next = new FloatRectangle(position.X+step.X, position.Z+step.Z, this.bounds.Width, this.bounds.Height);
                    //if (next.FloatIntersects(o.bounds))
                    if(this.checkBoxCollision)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<Waypoint> aStarToTarget(List<Waypoint> waypointsList)
        {
            List<Waypoint> pathToTake = new List<Waypoint>();

            // Find the nearest waypoint as a start point for the search
            float dist = float.MaxValue;
            foreach (Waypoint w in waypointsList)
            {
                float t = (this.position - w.position).LengthSquared();
                if (t < dist)
                {
                    this.nearestWaypoint = w;
                    dist = t;
                }
            }

            // Find the nearest waypoint to the player as a goal for the search
            dist = float.MaxValue;
            foreach (Waypoint w in waypointsList)
            {
                float t = (target.position - w.position).LengthSquared();
                if (t < dist)
                {
                    nearestToTarget = w;
                    dist = t;
                }
            }

            List<NodeRecord> openList = new List<NodeRecord>();
            List<NodeRecord> closedList = new List<NodeRecord>();
            NodeRecord current = new NodeRecord();

            NodeRecord startRecord = new NodeRecord();
            startRecord.currentWaypoint = nearestWaypoint;
            startRecord.costSoFar = 0;
            startRecord.fromWaypoint = null;
            startRecord.fromNode = new NodeRecord[1];
            startRecord.estimatedTotalCost = (startRecord.currentWaypoint.position - nearestToTarget.position).LengthSquared();

            openList.Add(startRecord);

            float endNodeCost = 0;
            float endNodeHeuristic = 0;
            while (openList.Count > 0)
            {
                float smallestCost = float.MaxValue;
                foreach(NodeRecord n in openList)
                    if (n.estimatedTotalCost < smallestCost)
                    {
                        current = n;
                        smallestCost = n.estimatedTotalCost;
                    }

                if (current.currentWaypoint.ID == nearestToTarget.ID)
                    break;

                foreach (Waypoint.Edge connection in current.currentWaypoint.connectedEdges)
                {
                    NodeRecord endNode = new NodeRecord();
                    endNode.currentWaypoint = connection.connectedTo;
                    endNode.fromWaypoint = current.currentWaypoint;
                    endNode.fromNode = new NodeRecord[1];
                    endNode.fromNode[0] = current;
                    endNodeCost = current.costSoFar + connection.length;

                    NodeRecord endNodeRecord = new NodeRecord();

                    if (containedInList(closedList, endNode))
                    {
                        endNodeRecord = new NodeRecord();

                        // Equivalent to closedList.Find(endNode)
                        foreach(NodeRecord n in closedList)
                        {
                            if (n.currentWaypoint.ID == endNode.currentWaypoint.ID)
                            {
                                endNodeRecord.costSoFar = n.costSoFar;
                                endNodeRecord.currentWaypoint = n.currentWaypoint;
                                endNodeRecord.estimatedTotalCost = n.estimatedTotalCost;
                                endNodeRecord.fromWaypoint = n.fromWaypoint;
                                endNodeRecord.fromNode = n.fromNode;
                            }

                        }

                        if (endNodeRecord.costSoFar <= endNodeCost)
                            continue;
                        
                        closedList.Remove(endNodeRecord);

                        endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                    }
                        
                    else if (containedInList(openList, endNode))
                    {
                        endNodeRecord = new NodeRecord();

                        // Equivalent to closedList.Find(endNode)
                        foreach (NodeRecord n in openList)
                        {
                            if (n.currentWaypoint.ID == endNode.currentWaypoint.ID)
                            {
                                endNodeRecord.costSoFar = n.costSoFar;
                                endNodeRecord.currentWaypoint = n.currentWaypoint;
                                endNodeRecord.estimatedTotalCost = n.estimatedTotalCost;
                                endNodeRecord.fromWaypoint = n.fromWaypoint;
                                endNodeRecord.fromNode = n.fromNode;
                            }
                        }

                        if (endNodeRecord.costSoFar <= endNodeCost)
                            continue;

                        endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                    }

                    else
                    {
                        endNodeRecord = endNode;

                        endNodeHeuristic = (endNode.currentWaypoint.position - nearestToTarget.position).LengthSquared();
                    }

                    endNodeRecord.costSoFar = endNodeCost;
                    endNodeRecord.currentWaypoint = connection.connectedTo;
                    endNodeRecord.fromWaypoint = endNode.fromWaypoint;
                    // might cause problem?
                    endNodeRecord.fromNode[0] = endNode.fromNode[0];
                    endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;

                    if(!containedInList(openList, endNode))
                        openList.Add(endNodeRecord);
                }

                openList.Remove(current);
                closedList.Add(current);
            }

            if (current.currentWaypoint.ID != nearestToTarget.ID)
                return pathToTake;
            else
            {
                while (current.fromNode != null)
                {
                    pathToTake.Add(current.currentWaypoint);
                    current = current.fromNode[0];
                }
                pathToTake.Add(nearestWaypoint);
                pathToTake.Reverse();
            }

            return pathToTake;
        }

        private bool containedInList(List<NodeRecord> list, NodeRecord node)
        {
            foreach(NodeRecord n in list)
                if (node.currentWaypoint.ID == n.currentWaypoint.ID)
                    return true;

            return false;
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget, List<Waypoint> waypointsList)
        {
            if (Game1.endGameSwarm)
            {
                attackDistanceLimit = attackDistanceLimitFinal;
                teamSize = teamSizeFinal;
            }

            #region Choose Target
            // target is SpawnPoint allows returning enemy to interrupt to chase player
            if (target == null || !target.isAlive || target is SpawnPoint)
            {
                // Create lists of targets to go after
                List<Building> buildings = new List<Building>();
                List<Player> players = new List<Player>();
                List<Tower> towers = new List<Tower>();

                // Fill in the lists
                foreach (GameComponent gc in game.Components)
                {
                    if (gc is Player)
                    {
                        players.Add((Player)gc);
                    }
                    if (!Game1.endGameSwarm)
                    {
                        if (gc is Map)
                        {
                            Map m = (Map)gc;
                            buildings = m.usableBuildings;
                        }
                        if (gc is Tower)
                        {
                            towers.Add((Tower)gc);
                        }
                    }
                }

                // Find a suitable target
                chooseTarget(buildings, players, towers);

                newTarget = true;
            }
            #endregion

            // If we have a target to chase after
            if (target != null)
            {
                // Follow path or chase player
                findTarget(gameTime, cameraTarget, waypointsList);

                #region Obstacle Avoidance
                collisionAvoidance2(colliders);

                /*
                //--------------------------------------------
                // Find the target that's closest to collision
                //--------------------------------------------

                // Store the first collision time
                float shortestTime = float.MaxValue;

                // Store the target that collides and other data
                Object firstTarget = null;
                float firstMinSeperation = 0;
                float firstDistance = 0;
                Vector3 firstRelativePos = Vector3.Zero;
                Vector3 firstRelativeVel = Vector3.Zero;

                // Store data for checking steps
                Vector3 relativePos = Vector3.Zero;
                Vector3 relativeVel = Vector3.Zero;
                float relativeSpeed = 0;
                float timeToCollision = 0;

                // Loop through each possible collider
                foreach (Object o in colliders)
                {
                    if (!(o is Bullet))
                    {
                        // Calculate the time to collision
                        relativePos = o.position - this.position;
                        if (o is Enemy || o is Player)
                        {
                            Character c = (Character)o;
                            relativeVel = c.velocity - this.velocity;
                        }
                        else
                        {
                            relativeVel = Vector3.Zero;
                        }
                        relativeSpeed = relativeVel.Length();
                        timeToCollision = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

                        // Check if there is going to be a collision at all
                        float distance = relativePos.Length();
                        float minSeperation = distance - relativeSpeed * timeToCollision;
                        // Use the either bounds.width or bounds.height as a simulated diameter 
                        // (choose the bigger of the two)
                        if (minSeperation > (o.bounds.Width > o.bounds.Height ? o.bounds.Width : o.bounds.Height))
                            continue;

                        // Check if it is the shortest
                        if (timeToCollision > 0 && timeToCollision < shortestTime)
                        {
                            // Store the time, target and other data
                            shortestTime = timeToCollision;
                            firstTarget = o;
                            firstMinSeperation = minSeperation;
                            firstDistance = distance;
                            firstRelativePos = relativePos;
                            firstRelativeVel = relativeVel;
                        }
                    }
                }

                //-----------------------
                // Calculate the steering
                //-----------------------

                // If we have a target
                if (firstTarget != null)
                {
                    // If we are going to hit exactly. or if we're already colliding
                    // Then do steering based on current position
                    if (firstMinSeperation <= 0 || firstDistance < (bounds.Width > bounds.Height ? bounds.Width : bounds.Height))
                        relativePos = firstTarget.position - this.position;
                    // Otherwise calculate the future relative position
                    else
                        relativePos = firstRelativePos + firstRelativeVel * shortestTime;

                    // Avoid the target
                    Vector3 adjustment = relativePos - this.position;
                    adjustment.Normalize();
                    this.velocity += adjustment;
                    this.velocity.Normalize();
                    this.velocity *= speed;
                    maintainVelocity = true;
                }
                */

                #endregion
            }
            weapons[selectedWeapon].Update(gameTime, colliders, cameraTarget);
            base.Update(gameTime, colliders, cameraTarget, waypointsList);
        }

        protected virtual void chooseTarget(List<Building> buildings, List<Player> players, List<Tower> towers)
        {
            target = null;
            float nearestTarget = float.MaxValue;
            float distSquared = 0;
            // 1 = Tower
            // 2 = Player
            int targetType = 0;

            // Check all the generators
            foreach (Generator g in buildings)
            {
                if (g.functional)
                {
                    distSquared = (this.position - g.position).LengthSquared();
                    if (distSquared < attackDistanceLimit && distSquared < nearestTarget)
                    {
                        nearestTarget = distSquared;
                        target = g;
                    }
                }
            }

            // Check all the towers
            foreach (Tower t in towers)
            {
                if (t.attackerNum < teamSize)
                {
                    distSquared = (this.position - t.position).LengthSquared();
                    if (distSquared < attackDistanceLimit && distSquared < nearestTarget)
                    {
                        nearestTarget = distSquared;
                        target = t;
                        targetType = 1;
                    }
                }
            }

            // Check all the players
            foreach (Player p in players)
            {
                if (p.attackerNum < teamSize)
                {
                    distSquared = (this.position - p.position).LengthSquared();
                    if (distSquared < attackDistanceLimit && distSquared < nearestTarget)
                    {
                        nearestTarget = distSquared;
                        target = p;
                        targetType = 2;
                    }
                }
            }

            if (targetType == 1)
            {
                Tower t = (Tower)target;
                ++t.attackerNum;
            }
            else if (targetType == 2)
            {
                Player p = (Player)target;
                ++p.attackerNum;
            }
        }

        private void findTarget(GameTime gameTime, Vector3 cameraTarget, List<Waypoint> waypointsList)
        {
            // If target acquired
            if (target != null)
            {
                // If the target is a new one
                if (newTarget)
                {
                    // Find path to target
                    pathToTarget = aStarToTarget(waypointsList);

                    // Get first subTarget goal
                    pathStep = 0;
                    if (pathToTarget.Count != 0)
                    {
                        if (pathStep < pathToTarget.Count)
                            subTarget = pathToTarget[pathStep++];
                        else
                        {
                            // If we have no more subtargets and our target is a spawnpoint
                            // We have returned to the spawnpoint so return to an aggressive state
                            if (this.target is SpawnPoint)
                            {
                                target = null;
                            }
                        }
                    }
                    else
                    {
                        target = null;
                        return;
                    }
                    newTarget = false;
                }

                // If the intended target is within reach
                float distToTargetSquared = (target.position - this.position).LengthSquared();
                float distToSubTargetSquared = (subTarget.position - this.position).LengthSquared();

                // If target is close enough, begin chasing the player
                if (!chasingPlayer && distToTargetSquared < (weapons[selectedWeapon].range * 6))
                    chasingPlayer = true;

                // If enemy is chasing the player
                if (chasingPlayer)
                {
                    // And player is close enough to attack
                    if (distToTargetSquared < weapons[selectedWeapon].range * weapons[selectedWeapon].range)
                    {
                        // Attack the player
                        velocity = Vector3.Zero;
                        weapons[selectedWeapon].shoot(this.position, lookDirection, true, gameTime, cameraTarget);
                        lookDirection = target.position - this.position;
                    }
                    // else if player is too far to continue chasing
                    else if (distToTargetSquared > attackDistanceLimit)
                    {
                        chasingPlayer = false;
                        target = this.spawnPoint;
                        newTarget = true;
                        Player p = (Player)target;
                        --p.attackerNum;
                    }
                    // eles if player is too far to attack
                    else
                    {
                        // Chase the player
                        velocity = target.position - this.position;
                        velocity.Normalize();
                        lookDirection = velocity;
                        //newTarget = true;
                        //chasingPlayer = false;
                    }
                }

                // Start following the path
                else if (distToSubTargetSquared < 1)
                {
                    // If there are still subTargets in the list
                    // Go to the next one
                    if (pathStep < pathToTarget.Count)
                        subTarget = pathToTarget[pathStep++];
                    // Otherwise head back to the spawnpoint
                    else if (distToSubTargetSquared <= 0.5)
                    {
                        velocity = Vector3.Zero;
                        target = this.spawnPoint;
                        subTarget = null;
                        newTarget = true;
                        if (target is Player)
                        {
                            Player p = (Player)target;
                            --p.attackerNum;
                        }
                        else if (target is Tower)
                        {
                            Tower t = (Tower)target;
                            --t.attackerNum;
                        }
                    }
                }

                // If target is not within reach, and next subgoal is not reached
                // keep moving towards current subgoal
                else
                {
                    velocity = subTarget.position - this.position;
                    velocity.Normalize();
                    lookDirection = velocity;
                }
            }
        }
    }
}
