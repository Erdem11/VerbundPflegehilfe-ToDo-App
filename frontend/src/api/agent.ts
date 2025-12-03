import axios, {AxiosError, type AxiosResponse} from 'axios';
import {toast} from 'react-toastify';
import type {ApiResult} from '../common/types';
import {API_BASE_URL} from '../config';

axios.defaults.baseURL = API_BASE_URL;

const responseBody = <T>(response: AxiosResponse<ApiResult<T>>) => response.data.data;

axios.interceptors.response.use(
  async (response) => {
    const result = response.data as ApiResult<object>;
    if (result && !result.succeeded) {
      const errorMsg = result.errors ? result.errors.join(', ') : 'An unknown error occurred';
      toast.error(errorMsg);
      return Promise.reject(errorMsg);
    }
    return response;
  },
  (error: AxiosError) => {
    const {data, status} = error.response as AxiosResponse || {};
    switch (status) {
      case 400:
        if (data.errors) {
          const modelStateErrors: string[] = [];
          for (const key in data.errors) {
            if (data.errors[key]) modelStateErrors.push(data.errors[key]);
          }
          toast.error(modelStateErrors.flat().join('\n'));
        } else {
          toast.error(data.title || 'Bad Request');
        }
        break;
      case 401:
        toast.error('Unauthorized');
        break;
      case 404:
        toast.error('Resource not found');
        break;
      case 500:
        toast.error('Server error');
        break;
      default:
        toast.error('Something went wrong');
        break;
    }
    return Promise.reject(error);
  }
);

export const requests = {
  get: <T>(url: string, params?: object) => axios.get<ApiResult<T>>(url, {params}).then(responseBody),
  post: <T>(url: string, body: object) => axios.post<ApiResult<T>>(url, body).then(responseBody),
  put: <T>(url: string, body: object) => axios.put<ApiResult<T>>(url, body).then(responseBody),
  del: <T>(url: string) => axios.delete<ApiResult<T>>(url).then(responseBody),
};