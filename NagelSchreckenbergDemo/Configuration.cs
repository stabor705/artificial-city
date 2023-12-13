using System;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class Configuration
    {
        public static bool PRINT_GRAPH_STRUCTURE = false;
        public static bool PRINT_GRAPH_STATE_ON_EACH_ITERATION = false;
        public static bool DEBUG = false;
        public static bool DEBUG_FULL = false;
        public static bool VALIDATION_SCRIPT_LOGS = true;

        public static double VEHICLE_SPAWN_PROB = 0.1;
        public static int VEHICLE_LENGTH = 5;
        public static int MAX_VEHICLES = 30;
        public static int MAX_VELOCITY = 6;
        public static double RANDOM_VELOCITY_DECREASE_PROB = 0.5;

        public static int PEDESTRIAN_CROSSING_MIN_COUNTDOWN = 240;
        public static int PEDESTRIAN_CROSSING_COUNTDOWN_VARIANCE = 120;
        public static double PEDESTRIAN_CROSSING_PROBABILITY = 0.03;

        public static int TIME_BETWEEN_ITERATIONS = 10; // in ms
        // TODO iteration time? this value now is hardcoded and equals to 60

        public static void DisplayConfiguration()
        {
            Console.WriteLine("---------------CONFIGURATION---------------");
            Console.WriteLine("PRINT_GRAPH_STRUCTURE: " + PRINT_GRAPH_STRUCTURE);
            Console.WriteLine("PRINT_GRAPH_STATE_ON_EACH_ITERATION: " + PRINT_GRAPH_STATE_ON_EACH_ITERATION);
            Console.WriteLine("DEBUG: " + DEBUG);
            Console.WriteLine("DEBUG_FULL: " + DEBUG_FULL);
            Console.WriteLine("VALIDATION_SCRIPT_LOGS: " + VALIDATION_SCRIPT_LOGS);

            Console.WriteLine("VEHICLE_SPAWN_PROB: " + VEHICLE_SPAWN_PROB);
            Console.WriteLine("VEHICLE_LENGTH: " + VEHICLE_LENGTH);
            Console.WriteLine("MAX_VEHICLES: " + MAX_VEHICLES);
            Console.WriteLine("MAX_VELOCITY: " + MAX_VELOCITY);
            Console.WriteLine("RANDOM_VELOCITY_DECREASE_PROB: " + RANDOM_VELOCITY_DECREASE_PROB);

            Console.WriteLine("PEDESTRIAN_CROSSING_MIN_COUNTDOWN: " + PEDESTRIAN_CROSSING_MIN_COUNTDOWN);
            Console.WriteLine("PEDESTRIAN_CROSSING_COUNTDOWN_VARIANCE: " + PEDESTRIAN_CROSSING_COUNTDOWN_VARIANCE);
            Console.WriteLine("PEDESTRIAN_CROSSING_PROBABILITY: " + PEDESTRIAN_CROSSING_PROBABILITY);

            Console.WriteLine("TIME_BETWEEN_ITERATIONS: " + TIME_BETWEEN_ITERATIONS);
            // TODO iteration time? this value now is hardcoded and equals to 60
            Console.WriteLine("-------------CONFIGURATION END-------------");
        }
    }
}
