import Head from 'next/head';
import Link from 'next/link';
import styles from '../styles/Home.module.css';
import { useEffect, useState } from 'react';
import Cookies from 'js-cookie';

export default function Home() {
  const [planId, setPlanId] = useState(null);

  useEffect(() => {
    const id = Cookies.get('workoutPlanId');
    setPlanId(id);
  }, []);

  return (
    <div className={styles.container}>
      <Head>
        <title>LiftSmart</title>
        <meta name="description" content="LiftSmart - the ultimate AI-powered training companion" />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main className={styles.main}>
        <h1 className={styles.title}>LiftSmart</h1>
        <h2 className={styles.subtitle}>The Ultimate AI-Powered Training Companion</h2>
        <p className={styles.description}>
          Take your workouts to the next level with LiftSmart, the ultimate AI-powered training companion. This groundbreaking app creates adaptive workout schedules in real time, tailoring each session to your performance and progress.
        </p>
        <p className={styles.description}>
          With intelligent adjustments on the fly, you’ll push your limits, optimise every rep, and achieve your fitness goals faster than ever.
        </p>
        <p className={styles.description}>
          Say goodbye to guesswork—log your weights effortlessly through our sleek, intuitive interface and get instant feedback to keep you on track. Dive into detailed analytics and insights to watch your strength soar over time.
        </p>
        <p className={styles.description}>
          Whether you’re a gym newbie or a seasoned lifter, LiftSmart delivers a personalised training experience like no other.
        </p>
        <p className={styles.description}>
          Unlock your full potential with the power of AI. Start your fitness journey with LiftSmart today!
        </p>
        <Link legacyBehavior href="/survey">
          <a className={styles.button}>Begin Your Journey</a>
        </Link>
        {planId ? (
          <div style={{ marginTop: '1rem' }}>
            <Link legacyBehavior href={`/plan?planId=${planId}`}>
              <a className={styles.button}>View Your Plan</a>
            </Link>
          </div>
        ) : null}
      </main>
    </div>
  );
}