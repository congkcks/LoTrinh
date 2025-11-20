# Dashboard API Documentation

## T?ng quan
Tài li?u này mô t? các API endpoint cho ph?n Dashboard c?a h? th?ng h?c ti?ng Anh, bao g?m 2 service:
- **LoTriinhHoc Service**: Qu?n lý khóa h?c, bài h?c và ti?n ?? h?c t?p chính
- **Service2**: Qu?n lý th?ng kê chi ti?t v? t? v?ng, ghi chú, highlights và các k? n?ng

---

## LoTriinhHoc Service - Dashboard Controller

### Base URL
```
/api/dashboard
```

### 1. L?y thông tin t?ng quan Dashboard
**Endpoint:** `GET /api/dashboard/home/{userId}`

**Mô t?:** L?y thông tin t?ng quan v? ti?n ?? h?c t?p c?a user, bao g?m th?ng kê bài h?c, ?i?m s?, k? n?ng và ?? xu?t bài h?c ti?p theo.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```json
{
  "summary": {
    "totalLessons": 100,
    "completedLessons": 45,
    "progressPercent": 45,
    "avgScore": 85.5,
    "lastAccess": "2024-01-20T10:30:00Z"
  },
  "skills": {
    "grammar": 12,
    "listening": 8,
    "reading": 15,
    "vocabulary": 234
  },
  "userData": {
    "notesCount": 67,
    "highlightCount": 123
  },
  "plans": [
    {
      "id": 1,
      "name": "K? ho?ch h?c c? b?n",
      "isActive": true,
      "createdDate": "2024-01-01T00:00:00Z"
    }
  ],
  "recommendation": {
    "id": 46,
    "title": "Present Perfect Tense"
  }
}
```

**Response Fields:**
- `summary`: Th?ng kê t?ng quan
  - `totalLessons`: T?ng s? bài h?c
  - `completedLessons`: S? bài h?c ?ã hoàn thành
  - `progressPercent`: Ph?n tr?m ti?n ?? hoàn thành
  - `avgScore`: ?i?m trung bình
  - `lastAccess`: L?n truy c?p cu?i cùng
- `skills`: Th?ng kê k? n?ng
  - `grammar`: S? bài ng? pháp ?ã hoàn thành
  - `listening`: S? bài nghe ?ã hoàn thành
  - `reading`: S? bài ??c ?ã hoàn thành
  - `vocabulary`: S? t? v?ng ?ã h?c
- `userData`: D? li?u ng??i dùng
  - `notesCount`: S? ghi chú
  - `highlightCount`: S? highlight
- `plans`: Danh sách k? ho?ch h?c t?p ?ang ho?t ??ng
- `recommendation`: ?? xu?t bài h?c ti?p theo

---

### 2. L?y danh sách khóa h?c
**Endpoint:** `GET /api/dashboard/courses/{userId}`

**Mô t?:** L?y danh sách t?t c? khóa h?c và ti?n ?? hoàn thành c?a user cho t?ng khóa h?c.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```json
[
  {
    "courseId": 1,
    "name": "English for Beginners",
    "totalLessons": 50,
    "completedLessons": 23,
    "progressPercent": 46
  },
  {
    "courseId": 2,
    "name": "Intermediate English",
    "totalLessons": 40,
    "completedLessons": 10,
    "progressPercent": 25
  }
]
```

**Response Fields:**
- `courseId`: ID khóa h?c
- `name`: Tên khóa h?c
- `totalLessons`: T?ng s? bài h?c trong khóa h?c
- `completedLessons`: S? bài h?c ?ã hoàn thành
- `progressPercent`: Ph?n tr?m ti?n ?? hoàn thành

---

### 3. L?y danh sách module trong khóa h?c
**Endpoint:** `GET /api/dashboard/course/{courseId}/modules/{userId}`

**Mô t?:** L?y thông tin chi ti?t v? các module trong m?t khóa h?c c? th? và ti?n ?? c?a user.

**Parameters:**
- `courseId` (int, path): ID c?a khóa h?c
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```json
{
  "courseId": 1,
  "name": "English for Beginners",
  "modules": [
    {
      "moduleId": 1,
      "name": "Basic Grammar",
      "totalLessons": 15,
      "completedLessons": 8,
      "progressPercent": 53
    },
    {
      "moduleId": 2,
      "name": "Common Vocabulary",
      "totalLessons": 20,
      "completedLessons": 15,
      "progressPercent": 75
    }
  ]
}
```

**Response Fields:**
- `courseId`: ID khóa h?c
- `name`: Tên khóa h?c
- `modules`: Danh sách module
  - `moduleId`: ID module
  - `name`: Tên module
  - `totalLessons`: T?ng s? bài h?c trong module
  - `completedLessons`: S? bài h?c ?ã hoàn thành
  - `progressPercent`: Ph?n tr?m ti?n ?? hoàn thành

**Error Response:**
- `404 Not Found`: Khóa h?c không t?n t?i

---

### 4. L?y danh sách bài h?c trong module
**Endpoint:** `GET /api/dashboard/module/{moduleId}/lessons/{userId}`

**Mô t?:** L?y danh sách t?t c? bài h?c trong m?t module c? th? và tr?ng thái hoàn thành c?a user.

**Parameters:**
- `moduleId` (int, path): ID c?a module
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```json
{
  "id": 1,
  "name": "Basic Grammar",
  "lessons": [
    {
      "id": 1,
      "title": "Present Simple Tense",
      "isCompleted": true,
      "progressPercent": 100
    },
    {
      "id": 2,
      "title": "Past Simple Tense",
      "isCompleted": false,
      "progressPercent": 30
    }
  ]
}
```

**Response Fields:**
- `id`: ID module
- `name`: Tên module
- `lessons`: Danh sách bài h?c
  - `id`: ID bài h?c
  - `title`: Tiêu ?? bài h?c
  - `isCompleted`: Tr?ng thái hoàn thành (true/false)
  - `progressPercent`: Ph?n tr?m ti?n ?? hoàn thành bài h?c

**Error Response:**
- `404 Not Found`: Module không t?n t?i

---

## Service2 - Dashboard Stats Controller

### Base URL
```
/api/dashboard-stats
```

### 1. L?y s? l??ng t? v?ng
**Endpoint:** `GET /api/dashboard-stats/vocabulary/{userId}`

**Mô t?:** L?y t?ng s? t? v?ng mà user ?ã h?c.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```
234
```

**Response Type:** `int` - S? l??ng t? v?ng

---

### 2. L?y s? l??ng ghi chú
**Endpoint:** `GET /api/dashboard-stats/notes/{userId}`

**Mô t?:** L?y t?ng s? ghi chú mà user ?ã t?o.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```
67
```

**Response Type:** `int` - S? l??ng ghi chú

---

### 3. L?y s? l??ng highlights
**Endpoint:** `GET /api/dashboard-stats/highlights/{userId}`

**Mô t?:** L?y t?ng s? highlights mà user ?ã t?o.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```
123
```

**Response Type:** `int` - S? l??ng highlights

---

### 4. L?y s? bài ng? pháp ?ã hoàn thành
**Endpoint:** `GET /api/dashboard-stats/grammar-completed/{userId}`

**Mô t?:** L?y s? l??ng bài t?p ng? pháp mà user ?ã hoàn thành.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```
12
```

**Response Type:** `int` - S? bài ng? pháp ?ã hoàn thành

---

### 5. L?y s? bài ??c ?ã hoàn thành
**Endpoint:** `GET /api/dashboard-stats/reading-completed/{userId}`

**Mô t?:** L?y s? l??ng bài t?p ??c hi?u mà user ?ã hoàn thành.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```
15
```

**Response Type:** `int` - S? bài ??c ?ã hoàn thành

---

### 6. L?y s? bài nghe ?ã hoàn thành
**Endpoint:** `GET /api/dashboard-stats/listening-completed/{userId}`

**Mô t?:** L?y s? l??ng bài t?p nghe mà user ?ã hoàn thành.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```
8
```

**Response Type:** `int` - S? bài nghe ?ã hoàn thành

---

### 7. L?y s? flashcard ?ã thành th?o
**Endpoint:** `GET /api/dashboard-stats/flashcard-mastered/{userId}`

**Mô t?:** L?y s? l??ng flashcard mà user ?ã thành th?o.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```
45
```

**Response Type:** `int` - S? flashcard ?ã thành th?o

---

### 8. L?y th?ng kê t?ng h?p
**Endpoint:** `GET /api/dashboard-stats/summary/{userId}`

**Mô t?:** L?y t?t c? th?ng kê c?a user trong m?t response duy nh?t.

**Parameters:**
- `userId` (int, path): ID c?a ng??i dùng

**Response:**
```json
{
  "vocabulary": 234,
  "notes": 67,
  "highlights": 123,
  "grammarCompleted": 12,
  "readingCompleted": 15,
  "listeningCompleted": 8,
  "flashcardMastered": 45
}
```

**Response Fields:**
- `vocabulary`: S? t? v?ng ?ã h?c
- `notes`: S? ghi chú ?ã t?o
- `highlights`: S? highlights ?ã t?o
- `grammarCompleted`: S? bài ng? pháp ?ã hoàn thành
- `readingCompleted`: S? bài ??c ?ã hoàn thành
- `listeningCompleted`: S? bài nghe ?ã hoàn thành
- `flashcardMastered`: S? flashcard ?ã thành th?o

---

## Error Handling

### Common Error Codes
- `200 OK`: Request thành công
- `404 Not Found`: Resource không t?n t?i
- `500 Internal Server Error`: L?i server

### Error Response Format
```json
{
  "error": "Resource not found",
  "message": "The requested course does not exist",
  "statusCode": 404
}
```

---

## Authentication & Authorization
- T?t c? các endpoint ??u yêu c?u authentication
- User ch? có th? truy c?p d? li?u c?a chính mình thông qua `userId` parameter
- C?n implement proper authorization middleware ?? ??m b?o security

---

## Rate Limiting
- Recommend implement rate limiting ?? tránh abuse
- Suggest limit: 100 requests/minute per user

---

## Caching Strategy
- Dashboard data có th? ???c cache trong 5-10 phút
- Stats data có th? ???c cache trong 1-2 phút
- S? d?ng Redis ho?c memory cache ?? improve performance