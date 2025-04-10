import { useEffect, useState } from 'react';
import axios from 'axios';
import styles from '../styles/Home.module.css';
import { useRouter } from 'next/router';

export default function Plan() {
  const [planData, setPlanData] = useState(null);
  const router = useRouter();

  useEffect(() => {
    const workoutPlanId = document.cookie
      .split('; ')
      .find((row) => row.startsWith('workoutPlanId='))
      ?.split('=')[1];

    if (!workoutPlanId) {
      alert('No workout plan found. Please create or load a plan.');
      router.push('/');
      return;
    }

    axios
      .get(`http://localhost:5000/api/values/workout-plans/${workoutPlanId}`)
      .then((response) => {
        setPlanData(response.data);
      })
      .catch((error) => {
        console.error('Error loading workout plan:', error);
        alert('Failed to load workout plan. Please try again.');
        router.push('/');
      });
  }, [router]);

  const handleCompletionToggle = (weekIndex, dayIndex, workoutIndex, setIndex) => {
    setPlanData((prevPlanData) => {
      const updatedPlan = JSON.parse(JSON.stringify(prevPlanData)); // Deep copy to ensure immutability
      const setToUpdate = updatedPlan.workoutPlan[weekIndex].days[dayIndex].workouts[workoutIndex].sets[setIndex];
      setToUpdate.completed = !setToUpdate.completed;
      return updatedPlan;
    });
  };

  const saveCompletionStatus = async () => {
    try {
      await axios.put(`http://localhost:5000/api/values`, planData);
      alert('Completion status updated successfully!');
    } catch (error) {
      console.error('Error saving completion status:', error);
      alert('Failed to save completion status. Please try again.');
    }
  };

  if (!planData || !planData.workoutPlan) {
    return <div className={styles.container}>Loading...</div>;
  }

  return (
    <div className={styles.container}>
      <h1>Workout Plan</h1>
      <p><strong>Plan ID:</strong> {planData.workoutPlanId}</p>
      <p><strong>Duration:</strong> {planData.planDurationWeeks} weeks</p>
      <p><strong>Training Days per Week:</strong> {planData.trainingDaysPerWeek}</p>
      <p><strong>Completed:</strong> {planData.completed ? 'Yes' : 'No'}</p>

      {planData.workoutPlan.map((week, weekIndex) => (
        <div key={weekIndex} className={styles.weekSection}>
          <h2>Week {week.week}</h2>
          {week.days.map((day, dayIndex) => (
            <div key={dayIndex} className={styles.daySection}>
              <h3>Day {day.day}</h3>
              {day.workouts.map((workout, workoutIndex) => (
                <div key={workoutIndex} className={styles.workoutSection}>
                  <h4>{workout.workoutName}</h4>
                  <p>{workout.description}</p>
                  <p><strong>Rest Time:</strong> {workout.restTimeSeconds} seconds</p>
                  <p><strong>Completed:</strong> {workout.completed ? 'Yes' : 'No'}</p>
                  <h5>Sets:</h5>
                  <ul>
                    {workout.sets.map((set, setIndex) => (
                      <li key={setIndex}>
                        <p><strong>Set {set.setNumber}:</strong> {set.reps} reps at {set.weight} kg</p>
                        <label>
                          <input
                            type="checkbox"
                            checked={set.completed}
                            onChange={() => handleCompletionToggle(weekIndex, dayIndex, workoutIndex, setIndex)}
                          />
                          Completed
                        </label>
                      </li>
                    ))}
                  </ul>
                </div>
              ))}
            </div>
          ))}
        </div>
      ))}
      <button onClick={saveCompletionStatus} className={styles.button}>Save Completion Status</button>
    </div>
  );
}