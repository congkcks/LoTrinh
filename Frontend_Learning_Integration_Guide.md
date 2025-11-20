# Frontend Integration Guide - Learning a Lesson

## Overview
This document provides detailed instructions on the flow and API calls required for the frontend when a user learns a lesson, from start to completion.

---

## Learning Flow Overview

```
1. Load Lesson Content ? 2. Start Lesson ? 3. Track Progress ? 4. Save Notes/Highlights ? 5. Complete Lesson
```

---

## 1. Load Lesson Content (Initialization)

### 1.1 Get Lesson Details
**API:** `GET /api/lessons/{lessonId}`

**Frontend Implementation:**
```javascript
const loadLessonContent = async (lessonId) => {
  try {
    const response = await fetch(`/api/lessons/${lessonId}`);
    const lessonData = await response.json();
    
    return {
      id: lessonData.id,
      title: lessonData.title,
      videos: lessonData.videos,
      exerciseTypes: lessonData.exerciseTypes
    };
  } catch (error) {
    console.error('Error loading lesson:', error);
  }
};
```

**Response Structure:**
```json
{
  "id": 1,
  "title": "Present Simple Tense",
  "videos": [
    {
      "id": 1,
      "title": "Introduction Video",
      "filePath": "/videos/lesson1-intro.mp4"
    }
  ],
  "exerciseTypes": [
    {
      "id": 1,
      "name": "Multiple Choice",
      "exercises": [
        {
          "id": 1,
          "question": "Choose the correct answer",
          "optionA": "is",
          "optionB": "are",
          "optionC": "am",
          "optionD": "be",
          "correctOption": "A",
          "explanation": "Use 'is' with singular subjects"
        }
      ]
    }
  ]
}
```

### 1.2 Get Lesson Vocabulary
**API:** `GET /api/vocabulary/{lessonId}`

**Frontend Implementation:**
```javascript
const loadVocabulary = async (lessonId) => {
  try {
    const response = await fetch(`/api/vocabulary/${lessonId}`);
    const vocabulary = await response.json();
    return vocabulary;
  } catch (error) {
    console.error('Error loading vocabulary:', error);
  }
};
```

### 1.3 Check Current Progress
**API:** `GET /api/user-lessons/{userId}/{lessonId}`

**Frontend Implementation:**
```javascript
const checkExistingProgress = async (userId, lessonId) => {
  try {
    const response = await fetch(`/api/user-lessons/${userId}/${lessonId}`);
    const progress = await response.json();
    
    if (progress) {
      return {
        progressPercent: progress.progressPercent,
        lastWatchedSecond: progress.lastWatchedSecond,
        isCompleted: progress.isCompleted,
        score: progress.score
      };
    }
    return null;
  } catch (error) {
    console.error('Error checking progress:', error);
  }
};
```

---

## 2. Start Lesson

### 2.1 Initialize Lesson Progress
**API:** `POST /api/user-lessons/start`

**Frontend Implementation:**
```javascript
const startLesson = async (userId, lessonId) => {
  try {
    const requestBody = {
      userId: userId,
      lessonId: lessonId,
      progressPercent: 0,
      isCompleted: false,
      lastWatchedSecond: 0,
      score: null,
      notes: null
    };

    const response = await fetch('/api/user-lessons/start', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(requestBody)
    });

    const result = await response.json();
    console.log('Lesson started:', result.message);
    return result.data;
  } catch (error) {
    console.error('Error starting lesson:', error);
  }
};
```

---

## 3. Track Progress During Learning

### 3.1 Update Learning Progress
**API:** `PUT /api/user-lessons/update-progress/{userId}/{lessonId}`

**Frontend Implementation:**
```javascript
const updateProgress = async (userId, lessonId, progressPercent) => {
  try {
    const requestBody = {
      progressPercent: progressPercent
    };

    const response = await fetch(`/api/user-lessons/update-progress/${userId}/${lessonId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(requestBody)
    });

    const result = await response.json();
    return result.data;
  } catch (error) {
    console.error('Error updating progress:', error);
  }
};

// Call periodically or on specific events
const trackLearningProgress = (userId, lessonId) => {
  let currentProgress = 0;
  
  // Example: Update progress every 30 seconds or on specific actions
  const progressInterval = setInterval(() => {
    currentProgress += 10; // Increment based on learning activity
    
    if (currentProgress <= 100) {
      updateProgress(userId, lessonId, currentProgress);
    }
    
    if (currentProgress >= 100) {
      clearInterval(progressInterval);
    }
  }, 30000); // 30 seconds
  
  return progressInterval;
};
```

### 3.2 Save Video Watch Position
**API:** `PUT /api/user-lessons/watch/{userId}/{lessonId}`

**Frontend Implementation:**
```javascript
const saveWatchPosition = async (userId, lessonId, currentSecond) => {
  try {
    const requestBody = {
      lastWatchedSecond: currentSecond
    };

    const response = await fetch(`/api/user-lessons/watch/${userId}/${lessonId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(requestBody)
    });

    return await response.json();
  } catch (error) {
    console.error('Error saving watch position:', error);
  }
};

// Video player event handler
const setupVideoProgressTracking = (videoElement, userId, lessonId) => {
  let saveTimeout;
  
  videoElement.addEventListener('timeupdate', () => {
    clearTimeout(saveTimeout);
    
    // Debounce to avoid too many API calls
    saveTimeout = setTimeout(() => {
      const currentTime = Math.floor(videoElement.currentTime);
      saveWatchPosition(userId, lessonId, currentTime);
    }, 2000); // Save every 2 seconds of no activity
  });
  
  // Save on pause
  videoElement.addEventListener('pause', () => {
    const currentTime = Math.floor(videoElement.currentTime);
    saveWatchPosition(userId, lessonId, currentTime);
  });
};
```

---

## 4. Notes & Highlights Management

### 4.1 Load Existing Notes and Highlights
**Frontend Implementation:**
```javascript
const loadUserNotesAndHighlights = async (userId, lessonId) => {
  try {
    const [notesResponse, highlightsResponse] = await Promise.all([
      fetch(`/api/user-notes/${userId}/${lessonId}`),
      fetch(`/api/user-highlights/${userId}/${lessonId}`)
    ]);
    
    const notes = await notesResponse.json();
    const highlights = await highlightsResponse.json();
    
    return { notes, highlights };
  } catch (error) {
    console.error('Error loading notes and highlights:', error);
  }
};
```

### 4.2 Add/Update Notes
**APIs:** 
- `POST /api/user-notes/add`
- `PUT /api/user-notes/update/{id}`

**Frontend Implementation:**
```javascript
const addNote = async (userId, lessonId, noteText, timestamp = null) => {
  try {
    const requestBody = {
      userId: userId,
      lessonId: lessonId,
      note: noteText,
      timestamp: timestamp
    };

    const response = await fetch('/api/user-notes/add', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(requestBody)
    });

    return await response.json();
  } catch (error) {
    console.error('Error adding note:', error);
  }
};

const updateNote = async (noteId, noteText) => {
  try {
    const requestBody = {
      note: noteText
    };

    const response = await fetch(`/api/user-notes/update/${noteId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(requestBody)
    });

    return await response.json();
  } catch (error) {
    console.error('Error updating note:', error);
  }
};
```

### 4.3 Add/Update Highlights
**APIs:**
- `POST /api/user-highlights/add`
- `PUT /api/user-highlights/update/{id}`

**Frontend Implementation:**
```javascript
const addHighlight = async (userId, lessonId, startIndex, endIndex, color = '#ffff00') => {
  try {
    const requestBody = {
      userId: userId,
      lessonId: lessonId,
      startIndex: startIndex,
      endIndex: endIndex,
      color: color
    };

    const response = await fetch('/api/user-highlights/add', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(requestBody)
    });

    return await response.json();
  } catch (error) {
    console.error('Error adding highlight:', error);
  }
};

// Text selection handler
const setupHighlightFeature = (textElement, userId, lessonId) => {
  textElement.addEventListener('mouseup', async () => {
    const selection = window.getSelection();
    if (selection.toString().length > 0) {
      const range = selection.getRangeAt(0);
      const startIndex = range.startOffset;
      const endIndex = range.endOffset;
      
      // Show highlight color picker or use default
      const color = '#ffff00'; // Default yellow
      
      await addHighlight(userId, lessonId, startIndex, endIndex, color);
      
      // Apply highlight to DOM
      applyHighlightToDOM(range, color);
    }
  });
};
```

---

## 5. Exercise Handling

### 5.1 Submit Exercise Answers
**Frontend Implementation:**
```javascript
const submitExerciseAnswers = async (exercises, userAnswers) => {
  let totalQuestions = 0;
  let correctAnswers = 0;
  
  exercises.forEach(exerciseType => {
    exerciseType.exercises.forEach(exercise => {
      totalQuestions++;
      if (userAnswers[exercise.id] === exercise.correctOption) {
        correctAnswers++;
      }
    });
  });
  
  const score = (correctAnswers / totalQuestions) * 100;
  return {
    totalQuestions,
    correctAnswers,
    score: Math.round(score * 100) / 100 // Round to 2 decimal places
  };
};

// Example exercise component
const ExerciseComponent = ({ exercises, onComplete }) => {
  const [userAnswers, setUserAnswers] = useState({});
  
  const handleSubmit = async () => {
    const result = await submitExerciseAnswers(exercises, userAnswers);
    onComplete(result);
  };
  
  return (
    <div>
      {exercises.map(exerciseType => (
        <div key={exerciseType.id}>
          <h3>{exerciseType.name}</h3>
          {exerciseType.exercises.map(exercise => (
            <div key={exercise.id}>
              <p>{exercise.question}</p>
              {['A', 'B', 'C', 'D'].map(option => (
                <label key={option}>
                  <input
                    type="radio"
                    name={`exercise_${exercise.id}`}
                    value={option}
                    onChange={(e) => setUserAnswers({
                      ...userAnswers,
                      [exercise.id]: e.target.value
                    })}
                  />
                  {exercise[`option${option}`]}
                </label>
              ))}
            </div>
          ))}
        </div>
      ))}
      <button onClick={handleSubmit}>Submit Exercises</button>
    </div>
  );
};
```

---

## 6. Complete Lesson

### 6.1 Finish Lesson
**API:** `PUT /api/user-lessons/finish/{userId}/{lessonId}`

**Frontend Implementation:**
```javascript
const finishLesson = async (userId, lessonId, finalScore) => {
  try {
    const requestBody = {
      score: finalScore
    };

    const response = await fetch(`/api/user-lessons/finish/${userId}/${lessonId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(requestBody)
    });

    const result = await response.json();
    return result;
  } catch (error) {
    console.error('Error finishing lesson:', error);
  }
};
```

---

## 7. Complete Learning Component Example

```javascript
import React, { useState, useEffect } from 'react';

const LessonLearningComponent = ({ userId, lessonId }) => {
  const [lessonData, setLessonData] = useState(null);
  const [progress, setProgress] = useState(0);
  const [isCompleted, setIsCompleted] = useState(false);
  const [exerciseResults, setExerciseResults] = useState(null);

  useEffect(() => {
    initializeLesson();
  }, [userId, lessonId]);

  const initializeLesson = async () => {
    // 1. Load lesson content
    const lesson = await loadLessonContent(lessonId);
    setLessonData(lesson);
    
    // 2. Check existing progress
    const existingProgress = await checkExistingProgress(userId, lessonId);
    
    if (existingProgress) {
      setProgress(existingProgress.progressPercent);
      setIsCompleted(existingProgress.isCompleted);
    } else {
      // 3. Start new lesson
      await startLesson(userId, lessonId);
    }
    
    // 4. Load notes and highlights
    const { notes, highlights } = await loadUserNotesAndHighlights(userId, lessonId);
    // Apply to UI...
  };

  const handleProgressUpdate = async (newProgress) => {
    setProgress(newProgress);
    await updateProgress(userId, lessonId, newProgress);
  };

  const handleExerciseComplete = async (results) => {
    setExerciseResults(results);
    
    // Update progress to 100% when exercises are completed
    await handleProgressUpdate(100);
    
    // Finish lesson with score
    const result = await finishLesson(userId, lessonId, results.score);
    setIsCompleted(true);
    
    // Show completion message/redirect
    console.log('Lesson completed:', result);
  };

  const handleAddNote = async (noteText) => {
    await addNote(userId, lessonId, noteText);
    // Refresh notes display
  };

  const handleAddHighlight = async (startIndex, endIndex, color) => {
    await addHighlight(userId, lessonId, startIndex, endIndex, color);
    // Apply highlight to DOM
  };

  if (!lessonData) {
    return <div>Loading lesson...</div>;
  }

  return (
    <div className="lesson-learning-container">
      <h1>{lessonData.title}</h1>
      
      {/* Progress Bar */}
      <div className="progress-bar">
        <div 
          className="progress-fill" 
          style={{ width: `${progress}%` }}
        ></div>
        <span>{progress}% Complete</span>
      </div>

      {/* Video Section */}
      {lessonData.videos.map(video => (
        <video 
          key={video.id}
          src={video.filePath}
          controls
          onLoadedMetadata={(e) => setupVideoProgressTracking(e.target, userId, lessonId)}
        />
      ))}

      {/* Content with Notes/Highlights */}
      <div 
        className="lesson-content"
        onMouseUp={() => {/* Handle text selection for highlights */}}
      >
        {/* Lesson content here */}
      </div>

      {/* Notes Section */}
      <div className="notes-section">
        <textarea 
          placeholder="Add your notes..."
          onBlur={(e) => handleAddNote(e.target.value)}
        />
      </div>

      {/* Exercises */}
      {!isCompleted && (
        <ExerciseComponent 
          exercises={lessonData.exerciseTypes}
          onComplete={handleExerciseComplete}
        />
      )}

      {/* Completion Status */}
      {isCompleted && (
        <div className="completion-message">
          <h2>Congratulations! Lesson Completed</h2>
          {exerciseResults && (
            <p>Your Score: {exerciseResults.score}%</p>
          )}
        </div>
      )}
    </div>
  );
};

export default LessonLearningComponent;
```

---

## 8. Error Handling & Best Practices

### 8.1 Error Handling
```javascript
const handleApiError = (error, context) => {
  console.error(`Error in ${context}:`, error);
  
  // Show user-friendly error message
  if (error.status === 404) {
    showNotification('Content not found', 'error');
  } else if (error.status >= 500) {
    showNotification('Server error. Please try again later.', 'error');
  } else {
    showNotification('Something went wrong. Please try again.', 'error');
  }
};
```

### 8.2 Offline Support
```javascript
const queueOfflineActions = (action, data) => {
  if (!navigator.onLine) {
    const offlineQueue = JSON.parse(localStorage.getItem('offlineQueue') || '[]');
    offlineQueue.push({ action, data, timestamp: Date.now() });
    localStorage.setItem('offlineQueue', JSON.stringify(offlineQueue));
    return true;
  }
  return false;
};

const processOfflineQueue = async () => {
  const offlineQueue = JSON.parse(localStorage.getItem('offlineQueue') || '[]');
  
  for (const item of offlineQueue) {
    try {
      await executeAction(item.action, item.data);
    } catch (error) {
      console.error('Failed to process offline action:', error);
    }
  }
  
  localStorage.removeItem('offlineQueue');
};
```

### 8.3 Performance Optimization
```javascript
// Debounce progress updates
const debouncedProgressUpdate = debounce(updateProgress, 1000);

// Cache lesson content
const lessonCache = new Map();

const loadLessonContentCached = async (lessonId) => {
  if (lessonCache.has(lessonId)) {
    return lessonCache.get(lessonId);
  }
  
  const lesson = await loadLessonContent(lessonId);
  lessonCache.set(lessonId, lesson);
  return lesson;
};
```

---

## 9. Testing Considerations

### 9.1 Unit Tests Example
```javascript
describe('Lesson Learning Flow', () => {
  test('should start lesson successfully', async () => {
    const result = await startLesson(1, 1);
    expect(result.userId).toBe(1);
    expect(result.lessonId).toBe(1);
    expect(result.progressPercent).toBe(0);
  });

  test('should update progress correctly', async () => {
    const result = await updateProgress(1, 1, 50);
    expect(result.progressPercent).toBe(50);
  });

  test('should complete lesson with score', async () => {
    const result = await finishLesson(1, 1, 85.5);
    expect(result.data.isCompleted).toBe(true);
    expect(result.data.score).toBe(85.5);
  });
});
```

---

## 10. API Endpoints Summary

### LoTriinhHoc Service
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/lessons/{id}` | Get lesson details with videos and exercises |
| GET | `/api/user-lessons/{userId}/{lessonId}` | Check existing progress |
| POST | `/api/user-lessons/start` | Start a new lesson |
| PUT | `/api/user-lessons/update-progress/{userId}/{lessonId}` | Update learning progress |
| PUT | `/api/user-lessons/watch/{userId}/{lessonId}` | Save video watch position |
| PUT | `/api/user-lessons/finish/{userId}/{lessonId}` | Complete lesson with score |

### Service2
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/vocabulary/{lessonId}` | Get lesson vocabulary |
| GET | `/api/user-notes/{userId}/{lessonId}` | Get user notes for lesson |
| POST | `/api/user-notes/add` | Add new note |
| PUT | `/api/user-notes/update/{id}` | Update existing note |
| DELETE | `/api/user-notes/delete/{id}` | Delete note |
| GET | `/api/user-highlights/{userId}/{lessonId}` | Get user highlights for lesson |
| POST | `/api/user-highlights/add` | Add new highlight |
| PUT | `/api/user-highlights/update/{id}` | Update existing highlight |
| DELETE | `/api/user-highlights/delete/{id}` | Delete highlight |

---

## 11. Authentication & Security

### 11.1 Authentication Headers
```javascript
const apiCall = async (url, options = {}) => {
  const token = localStorage.getItem('authToken');
  
  const defaultOptions = {
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    }
  };
  
  return fetch(url, { ...defaultOptions, ...options });
};
```

### 11.2 Data Validation
```javascript
const validateLessonData = (lessonData) => {
  if (!lessonData.id || !lessonData.title) {
    throw new Error('Invalid lesson data');
  }
  
  if (!Array.isArray(lessonData.exerciseTypes)) {
    throw new Error('Exercise types must be an array');
  }
  
  return true;
};
```

This comprehensive documentation provides frontend developers with all the necessary information to integrate with the backend APIs for creating a complete lesson learning experience.