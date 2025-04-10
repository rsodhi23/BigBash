import Head from 'next/head';
import Link from 'next/link';
import { useState } from 'react';
import Slider from 'rc-slider';
import 'rc-slider/assets/index.css';
import axios from 'axios';
import styles from '../styles/Home.module.css';

export default function Survey() {
  const [experienceLevel, setExperienceLevel] = useState('');
  const [trainingType, setTrainingType] = useState('');
  const [range, setRange] = useState([1, 7]);
  const [planDuration, setPlanDuration] = useState(6);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    const formData = {
      experienceLevel,
      trainingType,
      trainingDaysPerWeek: {
        min: range[0],
        max: range[1],
      },
      planDurationWeeks: planDuration,
      additionalDetails: document.querySelector('textarea').value || '',
    };

    try {
      await axios.post('http://localhost:5000/api/workout-plans', formData);
      const randomGuid = crypto.randomUUID();
      document.cookie = `workoutPlanId=${randomGuid}; path=/;`;
      alert('Plan created successfully!');
    } catch (error) {
      if (error.response) {
        if (error.response.status === 400 && error.response.data && error.response.data.body) {
          const { error: errorMessage } = error.response.data.body;
          throw new Error(`Invalid input: ${errorMessage}`);
        } else if (error.response.status === 500 && error.response.data && error.response.data.body) {
          const { error: errorMessage } = error.response.data.body;
          throw new Error(`Server error: ${errorMessage}`);
        }
      } else {
        console.error('Error submitting form:', error);
        alert('An error occurred while submitting the form. Please try again.');
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <Head>
        <title>Begin Your Journey - Survey</title>
        <meta name="description" content="Survey to begin your fitness journey with LiftSmart" />
      </Head>

      {isLoading && <div className={styles.spinner}>Loading...</div>}

      <main className={styles.main}>
        <h1 className={styles.title}>Begin Your Journey</h1>
        <p className={styles.description}>
          Take this quick survey to help us tailor your fitness journey with LiftSmart.
        </p>
        <div className={styles.form_section}>
          <h2 className={styles.subtitle}>Survey</h2>
          <form className={styles.form} onSubmit={handleSubmit}>
            <div className={styles.section}>
              <h2 className={styles.subtitle}>Personal Information</h2>
              <label>
                <span>What’s your experience level?</span>
                <div className={styles.graphicalCTA}>
                  <button
                    type="button"
                    className={experienceLevel === 'Beginner' ? styles.selected : ''}
                    onClick={() => setExperienceLevel('Beginner')}
                  >
                    Beginner
                  </button>
                  <button
                    type="button"
                    className={experienceLevel === 'Intermediate' ? styles.selected : ''}
                    onClick={() => setExperienceLevel('Intermediate')}
                  >
                    Intermediate
                  </button>
                  <button
                    type="button"
                    className={experienceLevel === 'Advanced' ? styles.selected : ''}
                    onClick={() => setExperienceLevel('Advanced')}
                  >
                    Advanced
                  </button>
                </div>
              </label>
              <label>
                <span>What is your sex?</span>
                <div className={styles.graphicalCTA}>
                  <button
                    type="button"
                    className={trainingType === 'Male' ? styles.selected : ''}
                    onClick={() => setTrainingType('Male')}
                  >
                    Male
                  </button>
                  <button
                    type="button"
                    className={trainingType === 'Female' ? styles.selected : ''}
                    onClick={() => setTrainingType('Female')}
                  >
                    Female
                  </button>
                  <button
                    type="button"
                    className={trainingType === 'Other' ? styles.selected : ''}
                    onClick={() => setTrainingType('Other')}
                  >
                    Other
                  </button>
                </div>
              </label>
              <label>
                <span>What is your age?</span>
                <input type="number" placeholder="Enter your age" min="18" max="120" />
              </label>
            </div>

            <div className={styles.section}>
              <h2 className={styles.subtitle}>Fitness Goals</h2>
              <label>
                <span>What is your goal?</span>
                <div className={styles.graphicalCTA}>
                  <button
                    type="button"
                    className={trainingType === 'Lose Weight' ? styles.selected : ''}
                    onClick={() => setTrainingType('Lose Weight')}
                  >
                    Lose Weight
                  </button>
                  <button
                    type="button"
                    className={trainingType === 'Gain Muscle' ? styles.selected : ''}
                    onClick={() => setTrainingType('Gain Muscle')}
                  >
                    Gain Muscle
                  </button>
                  <button
                    type="button"
                    className={trainingType === 'Get Fitter' ? styles.selected : ''}
                    onClick={() => setTrainingType('Get Fitter')}
                  >
                    Get Fitter
                  </button>
                </div>
              </label>
            </div>

            <div className={styles.section}>
              <h2 className={styles.subtitle}>Training Preferences</h2>
              <label>
                <span>Min/max training days per week</span>
                <Slider
                  range
                  min={1}
                  max={7}
                  step={1}
                  value={range}
                  onChange={(value) => setRange(value)}
                  className={styles.slider}
                  trackStyle={[{ backgroundColor: '#7AAEA1' }]} /* Match teal color */
                  handleStyle={[
                    { backgroundColor: '#7AAEA1', borderColor: '#5F8F87' },
                    { backgroundColor: '#7AAEA1', borderColor: '#5F8F87' }
                  ]}
                />
                <p>{`Selected range: ${range[0]} to ${range[1]} days`}</p>
              </label>
              <label>
                <span>How long would you like the plan to last?</span>
                <Slider
                  min={1}
                  max={3}
                  step={1}
                  value={planDuration}
                  onChange={(value) => setPlanDuration(value)}
                  className={styles.slider}
                  trackStyle={[{ backgroundColor: '#7AAEA1' }]} /* Match teal color */
                  handleStyle={{ backgroundColor: '#7AAEA1', borderColor: '#5F8F87' }}
                />
                <p>{`Selected duration: ${planDuration} weeks`}</p>
              </label>
              <label>
                <span>(Optional) Any other details:</span>
                <textarea placeholder="My goals are, I love x, I hate Y, I have a shoulder injury etc"></textarea>
              </label>
            </div>
            <button type="submit" className={styles.button}>Submit</button>
          </form>
        </div>
        <Link legacyBehavior href="/">
          <a className={styles.link}>Back to Home</a>
        </Link>
      </main>
    </div>
  );
}