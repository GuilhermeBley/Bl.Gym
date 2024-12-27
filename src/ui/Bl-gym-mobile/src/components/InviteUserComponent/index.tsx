import React from "react";
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Alert } from 'react-native';
import { Formik, FormikHelpers } from 'formik';
import * as Yup from 'yup';
import styles from "./styles";

interface InviteUserComponentProps {
    onSuccessfullyInvited: (email: string) => Promise<void>,
    gymName: string
}

interface FormValues {
    email: string;
}

const validationSchema = Yup.object().shape({
    email: Yup.string()
        .email('Invalid email address')
        .required('Email is required'),
});

const InviteUserComponent : React.FC<InviteUserComponentProps> = ({
    onSuccessfullyInvited,
    gymName
}) => {

    const handleSendInvite = async (
        values: FormValues,
        { setSubmitting, resetForm }: FormikHelpers<FormValues>
    ) => {
        try {
            await onSuccessfullyInvited(values.email);

            resetForm(); // Reset the form
        } catch (error) {
            Alert.alert('Error', 'Failed to send invitation. Please try again later.');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <View style={styles.container}>
            <Text style={styles.title}>Invite a User to {gymName}</Text>

            <Formik
            initialValues={{ email: '' }}
            validationSchema={validationSchema}
            onSubmit={handleSendInvite}
            >
            {({
                handleChange,
                handleBlur,
                handleSubmit,
                values,
                errors,
                touched,
                isSubmitting,
            }) => (
                <>
                <TextInput
                    style={styles.input}
                    placeholder="Enter email address"
                    value={values.email}
                    onChangeText={handleChange('email')}
                    onBlur={handleBlur('email')}
                    keyboardType="email-address"
                    autoCapitalize="none"
                />
                {touched.email && errors.email && (
                    <Text style={styles.errorText}>{errors.email}</Text>
                )}

                <TouchableOpacity
                    style={[styles.button, isSubmitting && styles.buttonDisabled]}
                    onPress={handleSubmit as () => void}
                    disabled={isSubmitting}
                >
                    <Text style={styles.buttonText}>{isSubmitting ? 'Sending...' : 'Send Invite'}</Text>
                </TouchableOpacity>
                </>
            )}
            </Formik>
        </View>
    );
}

export default InviteUserComponent;