// axiosConfig.js
import axios from 'axios';
import { API_BASE_URL } from '@env';

const instance = create();

function create(){
  console.debug('API URL: ' + API_BASE_URL);
  return axios.create({
    baseURL: API_BASE_URL,
  });
}

export default instance;